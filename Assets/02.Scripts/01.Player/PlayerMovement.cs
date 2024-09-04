using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [FoldoutGroup("References"), SerializeField, Required]
    private Rigidbody rb;

    [ShowInInspector, ReadOnly]
    private bool isBlocking = false; // 방어 중인지 여부

    [ShowInInspector, ReadOnly]
    private bool isDodging = false; // 회피 중인지 여부


    //===================================================================

    private void Start()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 초기화

    }

    //===================================================================

    #region 이동
    [Button("Handle Movement")]
    public void HandleMovement(Vector2 Direction, float moveSpeed, float blockMoveSpeed)
    {
        if (isDodging)
            return; // 구르기 중에는 이동하지 않음

        float moveHorizontal = Direction.x;
        float moveVertical = Direction.y;
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //if (movement != Vector3.zero) // 입력이 있을 때만 회전
        //{
        //    RotateTowardsMovementDirection(movement); // 이동 방향으로 회전
        //}

        rb.velocity = isBlocking ? movement * blockMoveSpeed : movement * moveSpeed; // 이동 속도 설정
    }
 
    #endregion

    #region 공격
    [Button("Attack")]
    public void Attack(float attackDamage)
    {
            Debug.Log("공격!"); // 공격 로직
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
        if(isBlocking == false)
        {
            return;
        }

        isBlocking = false;
        Debug.Log("가드 해제");
    }
    #endregion

    #region 회피
    [Button("Dodge")]
    public void Dodge(float dodgeForce, float dodgeDuration)
    {
        Debug.Log("구르기"); // 회피 로직
        StartCoroutine(DodgeRoutine(dodgeForce, dodgeDuration)); // 회피 코루틴 시작
    }

    private IEnumerator DodgeRoutine(float dodgeForce, float dodgeDuration)
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
