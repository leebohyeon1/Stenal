using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [FoldoutGroup("Enemy Settings"), SerializeField, Range(1, 20)]
    public float DetectionRange = 15f; // 플레이어 탐지 범위

    [FoldoutGroup("Enemy Settings"), SerializeField, Range(1, 5)]
    public float AttackRange = 2f; // 공격 범위

    [FoldoutGroup("Enemy Settings"), SerializeField, Range(0.1f, 3f)]
    public float AttackCooldown = 2f; // 공격 쿨다운 시간

    [FoldoutGroup("References"), SerializeField, Required]
    public NavMeshAgent Agent; // NavMeshAgent 컴포넌트

    [FoldoutGroup("References"), SerializeField, Required]
    public Transform Player; // 플레이어 Transform

    [ShowInInspector, ReadOnly]
    public IEnemyState CurrentState; // 현재 상태

    [ShowInInspector, ReadOnly]
    public IEnemyState ChaseState = new ChaseState(); // 추적 상태
    [ShowInInspector, ReadOnly]
    public IEnemyState AttackState = new AttackState(); // 공격 상태
    [ShowInInspector, ReadOnly]
    public IEnemyState IdleState = new IdleState(); // 대기 상태

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 초기화
        Player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 찾기

        CurrentState = IdleState; // 초기 상태 설정
        CurrentState.EnterState(this); // 초기 상태로 진입
    }

    private void Update()
    {
        CurrentState.UpdateState(this); // 현재 상태의 Update 메서드 호출
    }

    public void SwitchState(IEnemyState newState)
    {
        CurrentState.ExitState(this); // 현재 상태 종료
        CurrentState = newState; // 새로운 상태로 전환
        CurrentState.EnterState(this); // 새로운 상태 진입
    }
}
