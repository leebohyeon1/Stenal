using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [FoldoutGroup("Enemy Settings"), SerializeField, Range(1, 20)]
    public float DetectionRange = 15f; // �÷��̾� Ž�� ����

    [FoldoutGroup("Enemy Settings"), SerializeField, Range(1, 5)]
    public float AttackRange = 2f; // ���� ����

    [FoldoutGroup("Enemy Settings"), SerializeField, Range(0.1f, 3f)]
    public float AttackCooldown = 2f; // ���� ��ٿ� �ð�

    [FoldoutGroup("References"), SerializeField, Required]
    public NavMeshAgent Agent; // NavMeshAgent ������Ʈ

    [FoldoutGroup("References"), SerializeField, Required]
    public Transform Player; // �÷��̾� Transform

    [ShowInInspector, ReadOnly]
    public IEnemyState CurrentState; // ���� ����

    [ShowInInspector, ReadOnly]
    public IEnemyState ChaseState = new ChaseState(); // ���� ����
    [ShowInInspector, ReadOnly]
    public IEnemyState AttackState = new AttackState(); // ���� ����
    [ShowInInspector, ReadOnly]
    public IEnemyState IdleState = new IdleState(); // ��� ����

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>(); // NavMeshAgent ������Ʈ �ʱ�ȭ
        Player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ã��

        CurrentState = IdleState; // �ʱ� ���� ����
        CurrentState.EnterState(this); // �ʱ� ���·� ����
    }

    private void Update()
    {
        CurrentState.UpdateState(this); // ���� ������ Update �޼��� ȣ��
    }

    public void SwitchState(IEnemyState newState)
    {
        CurrentState.ExitState(this); // ���� ���� ����
        CurrentState = newState; // ���ο� ���·� ��ȯ
        CurrentState.EnterState(this); // ���ο� ���� ����
    }
}
