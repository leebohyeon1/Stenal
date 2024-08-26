using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : IEnemyState
{
    public void EnterState(EnemyAI enemy)
    {
        Debug.Log("Entering Attack State");
        enemy.Agent.isStopped = true; // 공격할 때는 NavMeshAgent 정지
        enemy.StartCoroutine(AttackCoroutine(enemy)); // 공격 코루틴 시작
    }

    public void UpdateState(EnemyAI enemy)
    {
        // 공격 중에는 추가 로직 필요 없음
    }

    public void ExitState(EnemyAI enemy)
    {
        Debug.Log("Exiting Attack State");
        enemy.Agent.isStopped = false; // 공격이 끝나면 NavMeshAgent 재개
    }

    private IEnumerator AttackCoroutine(EnemyAI enemy)
    {
        while (enemy.CurrentState is AttackState)
        {
            Debug.Log("Enemy attacks the player!");
            // 공격 애니메이션 또는 로직 추가 가능

            yield return new WaitForSeconds(enemy.AttackCooldown); // 공격 쿨다운 대기

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.position);
            if (distanceToPlayer > enemy.AttackRange)
            {
                enemy.SwitchState(enemy.ChaseState); // 공격 범위를 벗어나면 추적 상태로 전환
            }
        }
    }
}
