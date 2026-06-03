using UnityEngine;

/// <summary>
/// 고기 아이템 스폰을 관리합니다
/// </summary>
public class MeatSpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    public GameObject meatPrefab;
    public float spawnInterval = 5f; // 고기 스폰 간격
    private float spawnTimer = 0f;

    [Header("스폰 범위")]
    public float mapMinX = -50f;
    public float mapMaxX = 50f;
    public float mapMinY = -50f;
    public float mapMaxY = 50f;

    [Header("최대 고기 수")]
    public int maxMeats = 10;
    private int currentMeatCount = 0;

    private Transform playerTransform;
    private float minDistanceFromPlayer = 3f; // 플레이어로부터 최소 거리

    private void Start()
    {
        // 플레이어 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        if (meatPrefab == null)
        {
            Debug.LogError("MeatSpawner: meatPrefab이 설정되지 않았습니다!");
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver()) return;

        spawnTimer += Time.deltaTime;

        // 난이도에 따라 스폰 간격 조절
        float adjustedInterval = spawnInterval / (GameManager.Instance.GetDifficultyMultiplier() * 0.5f);

        if (spawnTimer >= adjustedInterval)
        {
            if (currentMeatCount < maxMeats)
            {
                SpawnMeat();
            }
            spawnTimer = 0f;
        }
    }

    /// <summary>
    /// 고기를 스폰합니다
    /// </summary>
    private void SpawnMeat()
    {
        if (meatPrefab == null) return;

        Vector3 spawnPos;
        bool validPosition = false;

        // 유효한 위치를 찾을 때까지 반복
        for (int i = 0; i < 10; i++)
        {
            float randomX = Random.Range(mapMinX, mapMaxX);
            float randomY = Random.Range(mapMinY, mapMaxY);
            spawnPos = new Vector3(randomX, randomY, 0);

            // 플레이어로부터 충분히 멀리 떨어져 있는지 확인
            if (playerTransform != null)
            {
                float distanceFromPlayer = Vector3.Distance(spawnPos, playerTransform.position);
                if (distanceFromPlayer >= minDistanceFromPlayer)
                {
                    validPosition = true;
                    break;
                }
            }
            else
            {
                validPosition = true;
                break;
            }
        }

        if (!validPosition) return;

        GameObject meat = Instantiate(meatPrefab, spawnPos, Quaternion.identity);
        currentMeatCount++;

        // 고기의 부모를 설정 (정리용)
        meat.transform.SetParent(transform);
    }

    /// <summary>
    /// 고기가 제거될 때 호출됩니다
    /// </summary>
    public void OnMeatDestroyed()
    {
        currentMeatCount--;
    }

    /// <summary>
    /// 맵 범위를 설정합니다
    /// </summary>
    public void SetMapBoundaries(float minX, float maxX, float minY, float maxY)
    {
        mapMinX = minX;
        mapMaxX = maxX;
        mapMinY = minY;
        mapMaxY = maxY;
    }
}
