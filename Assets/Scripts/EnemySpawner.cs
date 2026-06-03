using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float initialSpawnRate = 1f;
    [SerializeField] private float maxSpawnRate = 5f;
    [SerializeField] private int maxEnemies = 100;
    
    private float spawnTimer = 0f;
    private float currentSpawnRate;
    
    private void Start()
    {
        currentSpawnRate = initialSpawnRate;
    }
    
    private void Update()
    {
        if (GameManager.instance.IsGameOver()) return;
        
        // 시간에 따라 스폰율 증가
        currentSpawnRate = Mathf.Min(initialSpawnRate + (GameManager.instance.GetGameTime() * 0.1f), maxSpawnRate);
        
        spawnTimer -= Time.deltaTime;
        
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = 1f / currentSpawnRate;
        }
    }
    
    private void SpawnEnemy()
    {
        int enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount >= maxEnemies) return;
        
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 spawnPosition = player.position + new Vector3(
            Mathf.Cos(angle) * spawnRadius,
            Mathf.Sin(angle) * spawnRadius,
            0
        );
        
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
