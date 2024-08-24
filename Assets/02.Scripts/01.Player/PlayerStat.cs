using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [FoldoutGroup("Health Settings"), SerializeField, Range(0, 100)]
    [LabelText("체력")] private float health = 100f; // 체력

    [FoldoutGroup("Health Settings")]
    [LabelText("체력 회복량")] private float healthRecoveryAmount = 50f;

    [FoldoutGroup("Stamina Settings"), SerializeField, Range(0, 100)]
    [LabelText("현재 스테미나")] private float curStamina = 100f; // 스태미나

    [FoldoutGroup("Stamina Settings"), SerializeField, Range(0, 100)]
    [LabelText("최대 스테미나")] private float maxStamina = 100f; // 최대 스태미나

    [FoldoutGroup("Stamina Settings"), SerializeField, Range(0, 20)]
    [LabelText("초당 스테미나 회복량")] private float staminaRecoveryRate = 10f; // 스태미나 회복 속도

    [FoldoutGroup("Stamina Settings"), SerializeField, Range(0, 5)]
    [LabelText("스테미나 회복 지연 시간")] private float staminaRecoveryDelay = 1f; // 스태미나 회복 지연 시간

    [ShowInInspector, ReadOnly]
    private float staminaRecoveryTimer = 0f; // 스태미나 회복 타이머

    [ShowInInspector, ReadOnly]
    private bool canRecoverStamina = true; // 스태미나 회복 가능 여부

    [FoldoutGroup("References"), SerializeField, Required]
    private PlayerMovement playerMovement; // 이동 관련 컴포넌트 참조

    //===================================================================

    private void Start()
    {
        if(playerMovement == null) 
            playerMovement = GetComponent<PlayerMovement>(); // PlayerMovement 컴포넌트 초기화
    }

    //===================================================================

    [Button("Take Damage")]
    public void TakeDamage(float damage)
    {
        if (playerMovement.IsBlocking())
        {
            float reducedDamage = damage * 0.5f; // 방어 시 데미지 감소
            UseStamina(reducedDamage); // 스태미나 소모
            health -= reducedDamage;
        }
        else
        {
            health -= damage; // 방어하지 않은 상태에서는 체력만 감소
        }

        if (health <= 0)
        {
            Die(); // 플레이어 사망 처리
        }
    }

    [Button("Use Stamina")]
    public bool UseStamina(float amount)
    {
        if (curStamina >= amount)
        {
            curStamina -= amount;
            canRecoverStamina = false; // 스태미나 회복 일시 정지
            staminaRecoveryTimer = 0f; // 회복 지연 타이머 초기화
            return true;
        }
        return false;
    }

    [Button("Recover Health")]
    public void RecoverHealth()
    {
        Debug.Log("체력 회복!"); // 체력 회복 로직
        health = Mathf.Min(health + healthRecoveryAmount, 100f); // 최대 체력을 초과하지 않도록 설정
    }

    [Button("Recover Stamina")]
    public void RecoverStamina()
    {
        if (!canRecoverStamina)
        {
            staminaRecoveryTimer += Time.deltaTime;
            if (staminaRecoveryTimer >= staminaRecoveryDelay)
            {
                canRecoverStamina = true;
            }
        }

        if (canRecoverStamina && curStamina < maxStamina)
        {
            float recoveryRate = playerMovement.IsBlocking() ? staminaRecoveryRate * 0.5f : staminaRecoveryRate;
            curStamina += recoveryRate * Time.deltaTime;
            curStamina = Mathf.Min(curStamina, maxStamina);
        }
    }

    [Button("Die")]
    private void Die()
    {
        Debug.Log("플레이어 사망."); // 사망 로직
        // 게임 오버 처리 등 추가 로직
    }
}
