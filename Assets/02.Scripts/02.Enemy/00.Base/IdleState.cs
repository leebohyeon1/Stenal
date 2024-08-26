using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    public void EnterState(EnemyAI enemy)
    {
        Debug.Log("Entering Idle State");
        enemy.Agent.isStopped = true; // 대기 상태에서는 NavMeshAgent 정지
    }

    public void UpdateState(EnemyAI enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.position);
        if (distanceToPlayer <= enemy.DetectionRange)
        {
            enemy.SwitchState(enemy.ChaseState); // 탐지 범위에 들어오면 추적 상태로 전환
        }
    }

    public void ExitState(EnemyAI enemy)
    {
        Debug.Log("Exiting Idle State");
    }
}
