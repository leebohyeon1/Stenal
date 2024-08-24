using UnityEngine;
using Sirenix.OdinInspector;


public class PlayerInput : MonoBehaviour
{
    [FoldoutGroup("Dependencies"), SerializeField, Required]
    private PlayerMovement playerMovement; // �̵� �� ���� ���� ������Ʈ ����

    [FoldoutGroup("Dependencies"), SerializeField, Required]
    private PlayerStat playerStat; // ü�� �� ���¹̳� ���� ������Ʈ ����

    //===================================================================

    private void Start()
    {
        if(playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>(); // PlayerMovement ������Ʈ �ʱ�ȭ

        if (playerStat == null)
            playerStat = GetComponent<PlayerStat>(); // PlayerStat ������Ʈ �ʱ�ȭ
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() == GameState.Playing)
        {
            HandleInput(); // �Է� ó��
            playerStat.RecoverStamina(); // ���¹̳� ȸ�� ó��
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(); // ���� �Ͻ� ����
        }
    }

    //===================================================================

    private void HandleInput()
    {
        playerMovement.HandleMovement(); // �̵� �Է� ó��

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
        {
            playerMovement.Attack(); // ����
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.X))
        {
            playerMovement.StartBlock(); // ��� ����
        }

        if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.X))
        {
            playerMovement.StopBlock(); // ��� ����
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.Dodge(); // ȸ��
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.C))
        {
            playerStat.RecoverHealth(); // ü�� ȸ��
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerMovement.Interact(); // ��ȣ�ۿ�
        }

      
    }

    [Button("Pause Game")]
    private void PauseGame()
    {
        GameManager.Instance.PauseControl();
     
    }
}
