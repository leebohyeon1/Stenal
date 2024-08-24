using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : IEnemyState
{
    public void EnterState(EnemyAI enemy)
    {
        Debug.Log("Entering Attack State");
        enemy.Agent.isStopped = true; // ������ ���� NavMeshAgent ����
        enemy.StartCoroutine(AttackCoroutine(enemy)); // ���� �ڷ�ƾ ����
    }

    public void UpdateState(EnemyAI enemy)
    {
        // ���� �߿��� �߰� ���� �ʿ� ����
    }

    public void ExitState(EnemyAI enemy)
    {
        Debug.Log("Exiting Attack State");
        enemy.Agent.isStopped = false; // ������ ������ NavMeshAgent �簳
    }

    private IEnumerator AttackCoroutine(EnemyAI enemy)
    {
        while (enemy.CurrentState is AttackState)
        {
            Debug.Log("Enemy attacks the player!");
            // ���� �ִϸ��̼� �Ǵ� ���� �߰� ����

            yield return new WaitForSeconds(enemy.AttackCooldown); // ���� ��ٿ� ���

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.position);
            if (distanceToPlayer > enemy.AttackRange)
            {
                enemy.SwitchState(enemy.ChaseState); // ���� ������ ����� ���� ���·� ��ȯ
            }
        }
    }
}
