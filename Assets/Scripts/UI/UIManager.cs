using UnityEngine;
using TMPro;

/// <summary>
/// UI를 관리합니다
/// 체력, 점수, 시간, 경험치 바 등을 표시합니다
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI 요소")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI experienceText;
    public Image healthBar;
    public Image experienceBar;
    public TextMeshProUGUI gameOverText;

    private PlayerController playerController;
    private ExperienceSystem experienceSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // 플레이어 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerController = playerObj.GetComponent<PlayerController>();
            experienceSystem = playerObj.GetComponent<ExperienceSystem>();
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver()) return;

        UpdateUI();
    }

    /// <summary>
    /// UI를 업데이트합니다
    /// </summary>
    private void UpdateUI()
    {
        // 점수 표시
        if (scoreText != null)
        {
            scoreText.text = $"점수: {GameManager.Instance.GetScore()}";
        }

        // 시간 표시
        if (timeText != null)
        {
            float time = GameManager.Instance.GetGameTime();
            timeText.text = $"시간: {time:F1}초";
        }

        // 플레이어 체력 표시
        if (playerController != null)
        {
            if (healthText != null)
            {
                healthText.text = $"체력: {playerController.GetCurrentHealth():F0}/{playerController.GetMaxHealth():F0}";
            }

            if (healthBar != null)
            {
                healthBar.fillAmount = playerController.GetHealthPercentage();
            }
        }

        // 경험치 시스템 표시
        if (experienceSystem != null)
        {
            if (levelText != null)
            {
                levelText.text = $"레벨: {experienceSystem.GetCurrentLevel()}";
            }

            if (experienceText != null)
            {
                experienceText.text = $"경험치: {experienceSystem.GetCurrentExperience():F0}/{experienceSystem.GetNextLevelExperience():F0}";
            }

            if (experienceBar != null)
            {
                experienceBar.fillAmount = experienceSystem.GetExperiencePercentage();
            }
        }
    }

    /// <summary>
    /// 게임 오버 화면을 표시합니다
    /// </summary>
    public void ShowGameOver()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = $"게임 오버!\n최종 점수: {GameManager.Instance.GetScore()}\n" +
                               $"생존 시간: {GameManager.Instance.GetGameTime():F1}초\n" +
                               $"도달 레벨: {(experienceSystem != null ? experienceSystem.GetCurrentLevel() : 1)}\n\n" +
                               "R을 눌러 재시작";
        }
    }
}
