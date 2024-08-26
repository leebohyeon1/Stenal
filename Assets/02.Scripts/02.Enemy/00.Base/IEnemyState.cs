using UnityEngine;

public interface IEnemyState
{
    void EnterState(EnemyAI enemy); // ���¿� ������ �� ȣ��Ǵ� �޼���
    void UpdateState(EnemyAI enemy); // ���°� ���ӵǴ� ���� ȣ��Ǵ� �޼���
    void ExitState(EnemyAI enemy); // ���¿��� ��� �� ȣ��Ǵ� �޼���
}
