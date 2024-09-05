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

    private Camera mainCamera; // ���� ī�޶� ������ ������ ����

    #region ü�� ����
    [TabGroup("Tab", "ü�� ����"), SerializeField, Range(0, 100)]
    [LabelText("ü��")] private float health = 100f; // ü��

    [TabGroup("Tab", "ü�� ����"), SerializeField]
    [LabelText("ü�� ȸ����")] private float healthRecoveryAmount = 50f;
    #endregion

    #region ���׹̳� ����
    [TabGroup("Tab", "���׹̳� ����"), SerializeField, Range(0, 100)]
    [LabelText("���� ���׹̳�")] private float curStamina = 100f; // ���¹̳�

    [TabGroup("Tab", "���׹̳� ����"), SerializeField, Range(0, 100)]
    [LabelText("�ִ� ���׹̳�")] private float maxStamina = 100f; // �ִ� ���¹̳�

    [TabGroup("Tab", "���׹̳� ����"), SerializeField, Range(0, 20)]
    [LabelText("�ʴ� ���׹̳� ȸ����")] private float staminaRecoveryRate = 10f; // ���¹̳� ȸ�� �ӵ�

    [TabGroup("Tab", "���׹̳� ����"), SerializeField, Range(0, 5)]
    [LabelText("���׹̳� ȸ�� ���� �ð�")] private float staminaRecoveryDelay = 1f; // ���¹̳� ȸ�� ���� �ð�
    #endregion

    #region �ൿ ����
    [TabGroup("Tab", "�̵� ����"), SerializeField, Range(1, 10)]
    [LabelText("�̵� �ӵ�")] private float moveSpeed = 5f; // �̵� �ӵ�

    [TabGroup("Tab", "�̵� ����"), SerializeField, Range(1, 5)]
    [LabelText("��� �� �̵� �ӵ�")] private float blockMoveSpeed = 2f; // ��� �� �̵� �ӵ�

    [TabGroup("Tab", "���� ����"), SerializeField]
    [LabelText("���ݷ�")] private float attackDamage = 7f;
    
    [TabGroup("Tab", "���� ����"), SerializeField]
    [LabelText("���׹̳� �Ҹ�")] private float attackStamina = 10f;

    [TabGroup("Tab", "ȸ�� ����"), SerializeField, Range(1, 20)]
    [LabelText("ȸ�� ����")] private float dodgeForce = 10f; // ������ ��

    [TabGroup("Tab", "ȸ�� ����"), SerializeField, Range(0.1f, 2f)]
    [LabelText("ȸ�� �ð�")] private float dodgeDuration = 0.5f; // ȸ�� �ð�

    [TabGroup("Tab", "ȸ�� ����"), SerializeField, Range(1, 20)]
    [LabelText("���׹̳� �Ҹ�")] private float dodgeStamina = 15.0f;
    #endregion

    [ShowInInspector, ReadOnly]
    private float staminaRecoveryTimer = 0f; // ���¹̳� ȸ�� Ÿ�̸�

    [ShowInInspector, ReadOnly]
    private bool canRecoverStamina = true; // ���¹̳� ȸ�� ���� ����

    void Start()
    {
        if(playerController == null)
            playerController = GetComponent<PlayerController>();
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

        mainCamera = Camera.main; // ���� ī�޶� ��������
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
        if(playerController.attackInput && UseStamina(attackStamina)) // ���� �Է��� �ߴٸ�
        {
            playerMovement.Attack(attackDamage);
        }

        if (playerController.dodgeInput && UseStamina(dodgeStamina)) // ������ �Է��� �ߴٸ�
        {
            playerMovement.Dodge(dodgeForce,dodgeDuration);
        }

        if (playerController.defendInput) // ��� �Է��� �ߴٸ�
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
        playerMovement.HandleMovement(playerController.moveInput,moveSpeed,blockMoveSpeed); // �÷��̾� �̵�
    }

    public void HandleMouseLook() // �÷��̾� ȸ��
    {
        // ���콺 ��ġ�� ȭ�� ��ǥ���� ���� ��ǥ�� ��ȯ
        Ray mouseRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(mouseRay, out float rayDistance))
        {
            Vector3 pointToLook = mouseRay.GetPoint(rayDistance);
            Vector3 lookDirection = pointToLook - transform.position;
            lookDirection.y = 0; // Y�� ȸ���� �����Ͽ� ��鿡���� ȸ���ϵ��� ����

            if (lookDirection.sqrMagnitude > 0.1f)
            {
                // ��� ���콺 �������� ȸ���ϵ��� ����
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }

    [Button("Take Damage")]
    public void TakeDamage(float damage) // �ǰ� �Լ�
    {
        if (playerMovement.IsBlocking())
        {
            float reducedDamage = damage * 0.5f; // ��� �� ������ ����
            UseStamina(reducedDamage); // ���¹̳� �Ҹ�
            health -= reducedDamage;
        }
        else
        {
            health -= damage; // ������� ���� ���¿����� ü�¸� ����
        }

        if (health <= 0)
        {
            Die(); // �÷��̾� ��� ó��
        }
    } 

    [Button("Use Stamina")]
    public bool UseStamina(float amount) // ���׹̳� ���
    {
        if (curStamina >= amount)
        {
            curStamina -= amount;
            canRecoverStamina = false; // ���¹̳� ȸ�� �Ͻ� ����
            staminaRecoveryTimer = 0f; // ȸ�� ���� Ÿ�̸� �ʱ�ȭ
            return true;
        }
        return false;
    }

    [Button("Recover Health")]
    public void RecoverHealth() // ü�� ȸ��
    {
        Debug.Log("ü�� ȸ��!"); // ü�� ȸ�� ����
        health = Mathf.Min(health + healthRecoveryAmount, 100f); // �ִ� ü���� �ʰ����� �ʵ��� ����
    } 

    [Button("Recover Stamina")]
    public void RecoverStamina() // �ڵ� ���׹̳� ȸ��
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
        Debug.Log("�÷��̾� ���."); // ��� ����
        // ���� ���� ó�� �� �߰� ����
    }


    private void Pause()
    {
        if (playerController.pauseInput)
        {
            GameManager.Instance.PauseControl();
        }
    }
}
