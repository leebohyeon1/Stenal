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
    private bool isBlocking = false; // ��� ������ ����

    [ShowInInspector, ReadOnly]
    private bool isDodging = false; // ȸ�� ������ ����


    //===================================================================

    private void Start()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ �ʱ�ȭ

    }

    //===================================================================

    #region �̵�
    [Button("Handle Movement")]
    public void HandleMovement(Vector2 Direction, float moveSpeed, float blockMoveSpeed)
    {
        if (isDodging)
            return; // ������ �߿��� �̵����� ����

        float moveHorizontal = Direction.x;
        float moveVertical = Direction.y;
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //if (movement != Vector3.zero) // �Է��� ���� ���� ȸ��
        //{
        //    RotateTowardsMovementDirection(movement); // �̵� �������� ȸ��
        //}

        rb.velocity = isBlocking ? movement * blockMoveSpeed : movement * moveSpeed; // �̵� �ӵ� ����
    }
 
    #endregion

    #region ����
    [Button("Attack")]
    public void Attack(float attackDamage)
    {
            Debug.Log("����!"); // ���� ����
    }
    #endregion

    #region ���
    [Button("Start Block")]
    public void StartBlock()
    {
        isBlocking = true;
        Debug.Log("����!");
    }

    [Button("Stop Block")]
    public void StopBlock()
    {
        if(isBlocking == false)
        {
            return;
        }

        isBlocking = false;
        Debug.Log("���� ����");
    }
    #endregion

    #region ȸ��
    [Button("Dodge")]
    public void Dodge(float dodgeForce, float dodgeDuration)
    {
        Debug.Log("������"); // ȸ�� ����
        StartCoroutine(DodgeRoutine(dodgeForce, dodgeDuration)); // ȸ�� �ڷ�ƾ ����
    }

    private IEnumerator DodgeRoutine(float dodgeForce, float dodgeDuration)
    {
        isDodging = true; // ȸ�� ���� ����

        // �÷��̾ �ٶ󺸴� �������� ���� ����
        Vector3 dodgeDirection = transform.forward * dodgeForce;
        rb.AddForce(dodgeDirection, ForceMode.VelocityChange); // Rigidbody�� ���� ���Ͽ� ������ ����

        yield return new WaitForSeconds(dodgeDuration); // ������ ���� �ð� ���� ���

        rb.velocity = Vector3.zero;
        isDodging = false; // ������ ���� ����
    }
    #endregion

    [Button("Interact")]
    public void Interact()
    {
        Debug.Log("��ȣ�ۿ�"); // ��ȣ�ۿ� ����
    }

    [ShowInInspector]
    public bool IsBlocking()
    {
        return isBlocking;
    }
}
