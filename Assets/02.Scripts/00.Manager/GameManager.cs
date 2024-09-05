using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Loading, Playing, Paused, GameOver };

public class GameManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static GameManager Instance { get; private set; }

    [SerializeField, ReadOnly]
    private GameState currentState;

    //===================================================================

    private void Awake()
    {
        // �̱��� ���� ����
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // �ٸ� �ν��Ͻ��� �����ϸ� �� �ν��Ͻ��� �ı�
        }
        else
        {
            Instance = this; // �̱��� �ν��Ͻ��� ����
            DontDestroyOnLoad(this.gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //===================================================================

    #region Ÿ�� ��Ʈ�� ����
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale; // ���� �ð��� �ӵ� ����
    }

    public void PauseControl() // ��Ȳ�� ���� ���� �Ͻ�����
    {
        if (GetCurrentState() == GameState.Loading)
        {
            return;
        }

        if (GetCurrentState() == GameState.Paused)
        {
            ResumeTime();
            UIManager.Instance.OptionUISet(false);
            Debug.Log("���� �簳."); // �簳 ����
        }
        else
        {
            PauseTime();
            UIManager.Instance.OptionUISet();
            Debug.Log("���� �Ͻ� ����."); // �Ͻ� ���� ����
        }
    }

    public void PauseTime()
    {
        Time.timeScale = 0; // ���� �ð� ����
        SetGameState(GameState.Paused);

       
    }

    public void ResumeTime()
    {
        Time.timeScale = 1; // ���� �ð� �簳
        SetGameState(GameState.Playing);
    }


    #endregion

    #region ���� ���� ����
    [Button("SetGameState")]
    public void SetGameState(GameState state)
    {
        currentState = state; // ���� ���� ���� ����
    }

    public GameState GetCurrentState()
    {
        return currentState; // ���� ���� ���� ��ȯ
    }
    #endregion
}
