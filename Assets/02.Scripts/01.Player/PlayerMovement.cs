using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [TabGroup("Tab","�̵� ����"), SerializeField, Range(1, 10)]
    [LabelText("�̵� �ӵ�")] private float moveSpeed = 5f; // �̵� �ӵ�

    [TabGroup("Tab", "�̵� ����"), SerializeField, Range(1, 5)]
    [LabelText("��� �� �̵� �ӵ�")] private float blockMoveSpeed = 2f; // ��� �� �̵� �ӵ�

    [TabGroup("Tab", "ȸ�� ����"), SerializeField, Range(1, 20)]  
    [LabelText("ȸ�� ����")] private float dodgeForce = 10f; // ������ ��

    [TabGroup("Tab", "ȸ�� ����"), SerializeField, Range(0.1f, 2f)]
    [LabelText("ȸ�� �ð�")] private float dodgeDuration = 0.5f; // ȸ�� �ð�

    [TabGroup("Tab", "ȸ�� ����"), SerializeField, Range(1, 20)]
    [LabelText("���׹̳� �Ҹ�")] private float dodgeStamina = 15.0f;

    [FoldoutGroup("References"), SerializeField, Required]
    private Rigidbody rb;

    [ShowInInspector, ReadOnly]
    private bool isBlocking = false; // ��� ������ ����

    [ShowInInspector, ReadOnly]
    private bool isDodging = false; // ȸ�� ������ ����

    [FoldoutGroup("References"), SerializeField, Required]
    private PlayerStat playerStat; // ü�� �� ���¹̳� ���� ������Ʈ ����

    //===================================================================

    private void Start()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ �ʱ�ȭ

        if(playerStat == null)
            playerStat = GetComponent<PlayerStat>(); // PlayerStat ������Ʈ �ʱ�ȭ
    }

    //===================================================================

    #region �̵�
    [Button("Handle Movement")]
    public void HandleMovement()
    {
        if (isDodging)
            return; // ������ �߿��� �̵����� ����

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement != Vector3.zero) // �Է��� ���� ���� ȸ��
        {
            RotateTowardsMovementDirection(movement); // �̵� �������� ȸ��
        }

        rb.velocity = isBlocking ? movement * blockMoveSpeed : movement * moveSpeed; // �̵� �ӵ� ����
    }
    private void RotateTowardsMovementDirection(Vector3 movement)
    {
        // �̵� �������� �÷��̾� ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f); // �ε巴�� ȸ��
    }
    #endregion

    #region ����
    [Button("Attack")]
    public void Attack()
    {
        if (playerStat.UseStamina(10f))
        {
            Debug.Log("����!"); // ���� ����
        }
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
        isBlocking = false;
        Debug.Log("���� ����");
    }
    #endregion

    #region ȸ��
    [Button("Dodge")]
    public void Dodge()
    {
        if (playerStat.UseStamina(dodgeStamina))
        {
            Debug.Log("������"); // ȸ�� ����
            StartCoroutine(DodgeRoutine()); // ȸ�� �ڷ�ƾ ����
        }
    }

    private IEnumerator DodgeRoutine()
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
