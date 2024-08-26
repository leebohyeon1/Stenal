using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [TabGroup("Tab","이동 설정"), SerializeField, Range(1, 10)]
    [LabelText("이동 속도")] private float moveSpeed = 5f; // 이동 속도

    [TabGroup("Tab", "이동 설정"), SerializeField, Range(1, 5)]
    [LabelText("방어 중 이동 속도")] private float blockMoveSpeed = 2f; // 방어 중 이동 속도

    [TabGroup("Tab", "회피 설정"), SerializeField, Range(1, 20)]  
    [LabelText("회피 강도")] private float dodgeForce = 10f; // 구르기 힘

    [TabGroup("Tab", "회피 설정"), SerializeField, Range(0.1f, 2f)]
    [LabelText("회피 시간")] private float dodgeDuration = 0.5f; // 회피 시간

    [TabGroup("Tab", "회피 설정"), SerializeField, Range(1, 20)]
    [LabelText("스테미나 소모량")] private float dodgeStamina = 15.0f;

    [FoldoutGroup("References"), SerializeField, Required]
    private Rigidbody rb;

    [ShowInInspector, ReadOnly]
    private bool isBlocking = false; // 방어 중인지 여부

    [ShowInInspector, ReadOnly]
    private bool isDodging = false; // 회피 중인지 여부

    [FoldoutGroup("References"), SerializeField, Required]
    private PlayerStat playerStat; // 체력 및 스태미나 관련 컴포넌트 참조

    //===================================================================

    private void Start()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 초기화

        if(playerStat == null)
            playerStat = GetComponent<PlayerStat>(); // PlayerStat 컴포넌트 초기화
    }

    //===================================================================

    #region 이동
    [Button("Handle Movement")]
    public void HandleMovement()
    {
        if (isDodging)
            return; // 구르기 중에는 이동하지 않음

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement != Vector3.zero) // 입력이 있을 때만 회전
        {
            RotateTowardsMovementDirection(movement); // 이동 방향으로 회전
        }

        rb.velocity = isBlocking ? movement * blockMoveSpeed : movement * moveSpeed; // 이동 속도 설정
    }
    private void RotateTowardsMovementDirection(Vector3 movement)
    {
        // 이동 방향으로 플레이어 회전
        Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f); // 부드럽게 회전
    }
    #endregion

    #region 공격
    [Button("Attack")]
    public void Attack()
    {
        if (playerStat.UseStamina(10f))
        {
            Debug.Log("공격!"); // 공격 로직
        }
    }
    #endregion

    #region 방어
    [Button("Start Block")]
    public void StartBlock()
    {
        isBlocking = true;
        Debug.Log("가드!");
    }

    [Button("Stop Block")]
    public void StopBlock()
    {
        isBlocking = false;
        Debug.Log("가드 해제");
    }
    #endregion

    #region 회피
    [Button("Dodge")]
    public void Dodge()
    {
        if (playerStat.UseStamina(dodgeStamina))
        {
            Debug.Log("구르기"); // 회피 로직
            StartCoroutine(DodgeRoutine()); // 회피 코루틴 시작
        }
    }

    private IEnumerator DodgeRoutine()
    {
        isDodging = true; // 회피 상태 설정

        // 플레이어가 바라보는 방향으로 힘을 가함
        Vector3 dodgeDirection = transform.forward * dodgeForce;
        rb.AddForce(dodgeDirection, ForceMode.VelocityChange); // Rigidbody에 힘을 가하여 구르기 시작

        yield return new WaitForSeconds(dodgeDuration); // 구르기 지속 시간 동안 대기

        rb.velocity = Vector3.zero;
        isDodging = false; // 구르기 상태 해제
    }
    #endregion

    [Button("Interact")]
    public void Interact()
    {
        Debug.Log("상호작용"); // 상호작용 로직
    }

    [ShowInInspector]
    public bool IsBlocking()
    {
        return isBlocking;
    }
}
