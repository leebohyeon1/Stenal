using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [FoldoutGroup("Dependencies"), SerializeField, Required]
    private PlayerController playerController;
    [FoldoutGroup("Dependencies"), SerializeField, Required]
    private PlayerMovement playerMovement;

    private Camera mainCamera; // 메인 카메라 참조를 저장할 변수

    #region 체력 변수
    [TabGroup("Tab", "체력 설정"), SerializeField, Range(0, 100)]
    [LabelText("체력")] private float health = 100f; // 체력

    [TabGroup("Tab", "체력 설정"), SerializeField]
    [LabelText("체력 회복량")] private float healthRecoveryAmount = 50f;
    #endregion

    #region 스테미나 변수
    [TabGroup("Tab", "스테미나 설정"), SerializeField, Range(0, 100)]
    [LabelText("현재 스테미나")] private float curStamina = 100f; // 스태미나

    [TabGroup("Tab", "스테미나 설정"), SerializeField, Range(0, 100)]
    [LabelText("최대 스테미나")] private float maxStamina = 100f; // 최대 스태미나

    [TabGroup("Tab", "스테미나 설정"), SerializeField, Range(0, 20)]
    [LabelText("초당 스테미나 회복량")] private float staminaRecoveryRate = 10f; // 스태미나 회복 속도

    [TabGroup("Tab", "스테미나 설정"), SerializeField, Range(0, 5)]
    [LabelText("스테미나 회복 지연 시간")] private float staminaRecoveryDelay = 1f; // 스태미나 회복 지연 시간
    #endregion

    #region 행동 변수
    [TabGroup("Tab", "이동 설정"), SerializeField, Range(1, 10)]
    [LabelText("이동 속도")] private float moveSpeed = 5f; // 이동 속도

    [TabGroup("Tab", "이동 설정"), SerializeField, Range(1, 5)]
    [LabelText("방어 중 이동 속도")] private float blockMoveSpeed = 2f; // 방어 중 이동 속도

    [TabGroup("Tab", "공격 설정"), SerializeField]
    [LabelText("공격력")] private float attackDamage = 7f;
    
    [TabGroup("Tab", "공격 설정"), SerializeField]
    [LabelText("스테미나 소모량")] private float attackStamina = 10f;

    [TabGroup("Tab", "회피 설정"), SerializeField, Range(1, 20)]
    [LabelText("회피 강도")] private float dodgeForce = 10f; // 구르기 힘

    [TabGroup("Tab", "회피 설정"), SerializeField, Range(0.1f, 2f)]
    [LabelText("회피 시간")] private float dodgeDuration = 0.5f; // 회피 시간

    [TabGroup("Tab", "회피 설정"), SerializeField, Range(1, 20)]
    [LabelText("스테미나 소모량")] private float dodgeStamina = 15.0f;
    #endregion

    [ShowInInspector, ReadOnly]
    private float staminaRecoveryTimer = 0f; // 스태미나 회복 타이머

    [ShowInInspector, ReadOnly]
    private bool canRecoverStamina = true; // 스태미나 회복 가능 여부

    void Start()
    {
        if(playerController == null)
            playerController = GetComponent<PlayerController>();
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

        mainCamera = Camera.main; // 메인 카메라 가져오기
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAction();
        HandleMouseLook();

        RecoverStamina();

        Pause();
    }

    private void FixedUpdate()
    {
        PlayerHandleMovement();
    }

    void PlayerAction()
    {
        if(playerController.attackInput && UseStamina(attackStamina)) // 공격 입력을 했다면
        {
            playerMovement.Attack(attackDamage);
        }

        if (playerController.dodgeInput && UseStamina(dodgeStamina)) // 구르기 입력을 했다면
        {
            playerMovement.Dodge(dodgeForce,dodgeDuration);
        }

        if (playerController.defendInput) // 방어 입력을 했다면
        {
            playerMovement.StartBlock();
        }
        else
        {
            playerMovement.StopBlock();
        }

        if (playerController.interactInput)
        {
            playerMovement.Interact();
        }

        if (playerController.recoverHealthInput) 
        {
            RecoverHealth();

        }

    }

    void PlayerHandleMovement() 
    {
        playerMovement.HandleMovement(playerController.moveInput,moveSpeed,blockMoveSpeed); // 플레이어 이동
    }

    public void HandleMouseLook() // 플레이어 회전
    {
        // 마우스 위치를 화면 좌표에서 월드 좌표로 변환
        Ray mouseRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(mouseRay, out float rayDistance))
        {
            Vector3 pointToLook = mouseRay.GetPoint(rayDistance);
            Vector3 lookDirection = pointToLook - transform.position;
            lookDirection.y = 0; // Y축 회전을 방지하여 평면에서만 회전하도록 설정

            if (lookDirection.sqrMagnitude > 0.1f)
            {
                // 즉시 마우스 방향으로 회전하도록 설정
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }

    [Button("Take Damage")]
    public void TakeDamage(float damage) // 피격 함수
    {
        if (playerMovement.IsBlocking())
        {
            float reducedDamage = damage * 0.5f; // 방어 시 데미지 감소
            UseStamina(reducedDamage); // 스태미나 소모
            health -= reducedDamage;
        }
        else
        {
            health -= damage; // 방어하지 않은 상태에서는 체력만 감소
        }

        if (health <= 0)
        {
            Die(); // 플레이어 사망 처리
        }
    } 

    [Button("Use Stamina")]
    public bool UseStamina(float amount) // 스테미나 사용
    {
        if (curStamina >= amount)
        {
            curStamina -= amount;
            canRecoverStamina = false; // 스태미나 회복 일시 정지
            staminaRecoveryTimer = 0f; // 회복 지연 타이머 초기화
            return true;
        }
        return false;
    }

    [Button("Recover Health")]
    public void RecoverHealth() // 체력 회복
    {
        Debug.Log("체력 회복!"); // 체력 회복 로직
        health = Mathf.Min(health + healthRecoveryAmount, 100f); // 최대 체력을 초과하지 않도록 설정
    } 

    [Button("Recover Stamina")]
    public void RecoverStamina() // 자동 스테미나 회복
    {
        if (!canRecoverStamina)
        {
            staminaRecoveryTimer += Time.deltaTime;
            if (staminaRecoveryTimer >= staminaRecoveryDelay)
            {
                canRecoverStamina = true;
            }
        }

        if (canRecoverStamina && curStamina < maxStamina)
        {
            float recoveryRate = playerMovement.IsBlocking() ? staminaRecoveryRate * 0.5f : staminaRecoveryRate;
            curStamina += recoveryRate * Time.deltaTime;
            curStamina = Mathf.Min(curStamina, maxStamina);
        }
    }

    [Button("Die")]
    private void Die()
    {
        Debug.Log("플레이어 사망."); // 사망 로직
        // 게임 오버 처리 등 추가 로직
    }


    private void Pause()
    {
        if (playerController.pauseInput)
        {
            GameManager.Instance.PauseControl();
        }
    }
}
