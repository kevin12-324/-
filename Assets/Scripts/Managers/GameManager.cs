using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 전체를 관리하는 매니저
/// 게임 상태, 점수, 시간 등을 관리합니다.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("게임 상태")]
    private float gameTime = 0f;
    private int score = 0;
    private bool isGameOver = false;

    [Header("난이도")]
    public float difficultyMultiplier = 1f;
    private float difficultyTimer = 0f;
    public float difficultyIncreaseInterval = 30f; // 30초마다 난이도 증가

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (isGameOver) return;

        gameTime += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        // 일정 시간마다 난이도 증가
        if (difficultyTimer >= difficultyIncreaseInterval)
        {
            IncreaseDifficulty();
            difficultyTimer = 0f;
        }
    }

    /// <summary>
    /// 난이도를 증가시킵니다
    /// </summary>
    private void IncreaseDifficulty()
    {
        difficultyMultiplier += 0.1f;
        Debug.Log($"난이도 증가! 배수: {difficultyMultiplier:F1}x");
    }

    /// <summary>
    /// 점수를 추가합니다
    /// </summary>
    public void AddScore(int amount)
    {
        score += amount;
    }

    /// <summary>
    /// 게임 오버 처리
    /// </summary>
    public void GameOver()
    {
        isGameOver = true;
        Debug.Log($"게임 오버! 최종 점수: {score}, 생존 시간: {gameTime:F1}초");
        
        // 게임 오버 UI 표시 (나중에 구현)
        Time.timeScale = 0f; // 게임 일시 정지
    }

    /// <summary>
    /// 게임 재시작
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Getter 메서드들
    public float GetGameTime() => gameTime;
    public int GetScore() => score;
    public bool IsGameOver() => isGameOver;
    public float GetDifficultyMultiplier() => difficultyMultiplier;
}
