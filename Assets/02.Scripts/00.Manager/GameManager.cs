using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Loading, Playing, Paused, GameOver };

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    [SerializeField, ReadOnly]
    private GameState currentState;

    //===================================================================

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // 다른 인스턴스가 존재하면 이 인스턴스를 파괴
        }
        else
        {
            Instance = this; // 싱글톤 인스턴스를 설정
            DontDestroyOnLoad(this.gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //===================================================================

    #region 타임 컨트롤 설정
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale; // 게임 시간의 속도 설정
    }

    public void PauseControl() // 상황에 따른 게임 일시정지
    {
        if (GetCurrentState() == GameState.Loading)
        {
            return;
        }

        if (GetCurrentState() == GameState.Paused)
        {
            ResumeTime();
            UIManager.Instance.OptionUISet(false);
            Debug.Log("게임 재개."); // 재개 로직
        }
        else
        {
            PauseTime();
            UIManager.Instance.OptionUISet();
            Debug.Log("게임 일시 정지."); // 일시 정지 로직
        }
    }

    public void PauseTime()
    {
        Time.timeScale = 0; // 게임 시간 정지
        SetGameState(GameState.Paused);

       
    }

    public void ResumeTime()
    {
        Time.timeScale = 1; // 게임 시간 재개
        SetGameState(GameState.Playing);
    }


    #endregion

    #region 게임 상태 설정
    [Button("SetGameState")]
    public void SetGameState(GameState state)
    {
        currentState = state; // 현재 게임 상태 설정
    }

    public GameState GetCurrentState()
    {
        return currentState; // 현재 게임 상태 반환
    }
    #endregion
}
