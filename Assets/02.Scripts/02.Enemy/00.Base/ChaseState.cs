using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    public void EnterState(EnemyAI enemy)
    {
        Debug.Log("Entering Chase State");
        enemy.Agent.isStopped = false; // ���� �̵��� �� �ֵ��� NavMeshAgent ����
    }

    public void UpdateState(EnemyAI enemy)
    {
        enemy.Agent.SetDestination(enemy.Player.position); // ���� �÷��̾ �����ϵ��� ��ǥ ����

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.position);
        if (distanceToPlayer <= enemy.AttackRange)
        {
            enemy.SwitchState(enemy.AttackState); // ���� ������ ������ ���� ���·� ��ȯ
        }
        else if (distanceToPlayer > enemy.DetectionRange)
        {
            enemy.SwitchState(enemy.IdleState); // Ž�� ������ ����� ��� ���·� ��ȯ
        }
    }

    public void ExitState(EnemyAI enemy)
    {
        Debug.Log("Exiting Chase State");
    }
}
