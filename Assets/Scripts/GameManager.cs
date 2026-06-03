using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] private float gameTime = 0f;
    [SerializeField] private int score = 0;
    [SerializeField] private int level = 1;
    private bool isGameOver = false;
    
    public delegate void OnGameOverDelegate();
    public event OnGameOverDelegate OnGameOver;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Update()
    {
        if (!isGameOver)
        {
            gameTime += Time.deltaTime;
        }
    }
    
    public void AddScore(int amount)
    {
        score += amount;
    }
    
    public void LevelUp()
    {
        level++;
    }
    
    public void GameOver()
    {
        isGameOver = true;
        OnGameOver?.Invoke();
    }
    
    public float GetGameTime() => gameTime;
    public int GetScore() => score;
    public int GetLevel() => level;
    public bool IsGameOver() => isGameOver;
}
