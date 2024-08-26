using UnityEngine;
using Sirenix.OdinInspector;


public class PlayerInput : MonoBehaviour
{
    [FoldoutGroup("Dependencies"), SerializeField, Required]
    private PlayerMovement playerMovement; // 이동 및 전투 관련 컴포넌트 참조

    [FoldoutGroup("Dependencies"), SerializeField, Required]
    private PlayerStat playerStat; // 체력 및 스태미나 관련 컴포넌트 참조

    //===================================================================

    private void Start()
    {
        if(playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>(); // PlayerMovement 컴포넌트 초기화

        if (playerStat == null)
            playerStat = GetComponent<PlayerStat>(); // PlayerStat 컴포넌트 초기화
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() == GameState.Playing)
        {
            HandleInput(); // 입력 처리
            playerStat.RecoverStamina(); // 스태미나 회복 처리
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(); // 게임 일시 정지
        }
    }

    //===================================================================

    private void HandleInput()
    {
        playerMovement.HandleMovement(); // 이동 입력 처리

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
        {
            playerMovement.Attack(); // 공격
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.X))
        {
            playerMovement.StartBlock(); // 방어 시작
        }

        if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.X))
        {
            playerMovement.StopBlock(); // 방어 중지
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.Dodge(); // 회피
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.C))
        {
            playerStat.RecoverHealth(); // 체력 회복
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerMovement.Interact(); // 상호작용
        }

      
    }

    [Button("Pause Game")]
    private void PauseGame()
    {
        GameManager.Instance.PauseControl();
     
    }
}
