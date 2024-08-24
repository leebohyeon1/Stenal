using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    public void EnterState(EnemyAI enemy)
    {
        Debug.Log("Entering Idle State");
        enemy.Agent.isStopped = true; // ��� ���¿����� NavMeshAgent ����
    }

    public void UpdateState(EnemyAI enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.position);
        if (distanceToPlayer <= enemy.DetectionRange)
        {
            enemy.SwitchState(enemy.ChaseState); // Ž�� ������ ������ ���� ���·� ��ȯ
        }
    }

    public void ExitState(EnemyAI enemy)
    {
        Debug.Log("Exiting Idle State");
    }
}
