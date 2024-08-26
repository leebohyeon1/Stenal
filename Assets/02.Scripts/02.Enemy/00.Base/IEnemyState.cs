using UnityEngine;

public interface IEnemyState
{
    void EnterState(EnemyAI enemy); // 상태에 진입할 때 호출되는 메서드
    void UpdateState(EnemyAI enemy); // 상태가 지속되는 동안 호출되는 메서드
    void ExitState(EnemyAI enemy); // 상태에서 벗어날 때 호출되는 메서드
}
