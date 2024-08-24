using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [FoldoutGroup("Health Settings"), SerializeField, Range(0, 100)]
    [LabelText("ü��")] private float health = 100f; // ü��

    [FoldoutGroup("Health Settings")]
    [LabelText("ü�� ȸ����")] private float healthRecoveryAmount = 50f;

    [FoldoutGroup("Stamina Settings"), SerializeField, Range(0, 100)]
    [LabelText("���� ���׹̳�")] private float curStamina = 100f; // ���¹̳�

    [FoldoutGroup("Stamina Settings"), SerializeField, Range(0, 100)]
    [LabelText("�ִ� ���׹̳�")] private float maxStamina = 100f; // �ִ� ���¹̳�

    [FoldoutGroup("Stamina Settings"), SerializeField, Range(0, 20)]
    [LabelText("�ʴ� ���׹̳� ȸ����")] private float staminaRecoveryRate = 10f; // ���¹̳� ȸ�� �ӵ�

    [FoldoutGroup("Stamina Settings"), SerializeField, Range(0, 5)]
    [LabelText("���׹̳� ȸ�� ���� �ð�")] private float staminaRecoveryDelay = 1f; // ���¹̳� ȸ�� ���� �ð�

    [ShowInInspector, ReadOnly]
    private float staminaRecoveryTimer = 0f; // ���¹̳� ȸ�� Ÿ�̸�

    [ShowInInspector, ReadOnly]
    private bool canRecoverStamina = true; // ���¹̳� ȸ�� ���� ����

    [FoldoutGroup("References"), SerializeField, Required]
    private PlayerMovement playerMovement; // �̵� ���� ������Ʈ ����

    //===================================================================

    private void Start()
    {
        if(playerMovement == null) 
            playerMovement = GetComponent<PlayerMovement>(); // PlayerMovement ������Ʈ �ʱ�ȭ
    }

    //===================================================================

    [Button("Take Damage")]
    public void TakeDamage(float damage)
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
    public bool UseStamina(float amount)
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
    public void RecoverHealth()
    {
        Debug.Log("ü�� ȸ��!"); // ü�� ȸ�� ����
        health = Mathf.Min(health + healthRecoveryAmount, 100f); // �ִ� ü���� �ʰ����� �ʵ��� ����
    }

    [Button("Recover Stamina")]
    public void RecoverStamina()
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
}
