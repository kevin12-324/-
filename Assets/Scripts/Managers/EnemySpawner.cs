using UnityEngine;

/// <summary>
/// 적 스폰을 관리합니다
/// 화면 밖에서 적을 스폰하여 플레이어를 향해 이동하게 합니다
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    private float spawnTimer = 0f;
    
    [Header("스폰 범위")]
    public float spawnDistance = 15f; // 플레이어로부터의 거리

    [Header("최대 적 수")]
    public int maxEnemies = 20;
    private int currentEnemyCount = 0;

    private Transform playerTransform;
    private Camera mainCamera;

    private void Start()
    {
        // 플레이어 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        mainCamera = Camera.main;

        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner: enemyPrefab이 설정되지 않았습니다!");
        }
    }

    private void Update()
    {
        if (playerTransform == null || GameManager.Instance.IsGameOver()) return;

        spawnTimer += Time.deltaTime;

        // 난이도에 따라 스폰 간격 단축
        float adjustedInterval = spawnInterval / GameManager.Instance.GetDifficultyMultiplier();

        if (spawnTimer >= adjustedInterval)
        {
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
            spawnTimer = 0f;
        }

        // 최대 적 수 업데이트 (난이도에 따라)
        maxEnemies = Mathf.RoundToInt(20 * GameManager.Instance.GetDifficultyMultiplier());
    }

    /// <summary>
    /// 적을 화면 밖에 스폰합니다
    /// </summary>
    private void SpawnEnemy()
    {
        if (enemyPrefab == null || playerTransform == null) return;

        // 플레이어 주변의 랜덤 방향 (화면 밖)
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        
        // 카메라 경계 밖에서 스폰하도록 계산
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float cameraHalfHeight = mainCamera.orthographicSize;
        
        // 스폰 위치 (카메라 범위 밖)
        Vector3 spawnPos = playerTransform.position + (Vector3)randomDirection * (spawnDistance + Mathf.Max(cameraHalfWidth, cameraHalfHeight));
        
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemyCount++;

        // 적의 부모를 설정 (정리용)
        enemy.transform.SetParent(transform);
    }

    /// <summary>
    /// 현재 스폰된 적의 수를 반환합니다
    /// </summary>
    public int GetCurrentEnemyCount() => currentEnemyCount;

    /// <summary>
    /// 적이 제거될 때 호출됩니다
    /// </summary>
    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
    }
}
