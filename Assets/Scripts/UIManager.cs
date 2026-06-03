using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject gameOverPanel;
    
    private Player player;
    
    private void Start()
    {
        player = FindObjectOfType<Player>();
        GameManager.instance.OnGameOver += ShowGameOver;
    }
    
    private void Update()
    {
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {GameManager.instance.GetScore()}";
        
        if (timeText != null)
            timeText.text = $"Time: {Mathf.FloorToInt(GameManager.instance.GetGameTime())}s";
        
        if (levelText != null)
            levelText.text = $"Level: {GameManager.instance.GetLevel()}";
        
        if (player != null)
        {
            if (healthText != null)
                healthText.text = $"HP: {player.GetHealth():F0}/{player.GetMaxHealth():F0}";
            
            if (healthBar != null)
                healthBar.fillAmount = player.GetHealth() / player.GetMaxHealth();
        }
    }
    
    private void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}
