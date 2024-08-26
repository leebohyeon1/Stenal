using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    public void EnterState(EnemyAI enemy)
    {
        Debug.Log("Entering Chase State");
        enemy.Agent.isStopped = false; // 적이 이동할 수 있도록 NavMeshAgent 시작
    }

    public void UpdateState(EnemyAI enemy)
    {
        enemy.Agent.SetDestination(enemy.Player.position); // 적이 플레이어를 추적하도록 목표 설정

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.position);
        if (distanceToPlayer <= enemy.AttackRange)
        {
            enemy.SwitchState(enemy.AttackState); // 공격 범위에 들어오면 공격 상태로 전환
        }
        else if (distanceToPlayer > enemy.DetectionRange)
        {
            enemy.SwitchState(enemy.IdleState); // 탐지 범위를 벗어나면 대기 상태로 전환
        }
    }

    public void ExitState(EnemyAI enemy)
    {
        Debug.Log("Exiting Chase State");
    }
}
