using UnityEngine;

/// <summary>
/// 카메라가 플레이어를 부드럽게 따라갑니다
/// 오픈형 맵에 최적화됨
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("카메라 설정")]
    public Transform target; // 따라갈 대상 (플레이어)
    public float smoothSpeed = 5f; // 카메라 이동 부드러움
    public Vector3 offset = new Vector3(0, 0, -10f); // 카메라 오프셋
    
    [Header("맵 경계")]
    public float mapMinX = -50f;
    public float mapMaxX = 50f;
    public float mapMinY = -50f;
    public float mapMaxY = 50f;
    public bool useMapBoundaries = true; // 맵 경계 사용 여부

    [Header("카메라 흔들림")]
    public bool enableScreenShake = true;
    private float shakeAmount = 0f;
    private float shakeIntensity = 0.5f;

    private Vector3 velocity = Vector3.zero; // SmoothDamp용
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 목표 위치 계산
        Vector3 targetPos = target.position + offset;

        // 맵 경계 내로 제한
        if (useMapBoundaries)
        {
            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            float cameraHalfHeight = mainCamera.orthographicSize;

            targetPos.x = Mathf.Clamp(targetPos.x, mapMinX + cameraHalfWidth, mapMaxX - cameraHalfWidth);
            targetPos.y = Mathf.Clamp(targetPos.y, mapMinY + cameraHalfHeight, mapMaxY - cameraHalfHeight);
        }

        // 스크린 쉐이크 적용
        if (enableScreenShake && shakeAmount > 0)
        {
            targetPos += (Vector3)Random.insideUnitCircle * shakeAmount;
            shakeAmount -= Time.deltaTime * shakeIntensity;
        }

        // 부드럽게 카메라 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 1f / smoothSpeed);
    }

    /// <summary>
    /// 카메라를 초기화합니다
    /// </summary>
    public void SetupCamera(Transform newTarget)
    {
        target = newTarget;
    }

    /// <summary>
    /// 스크린 쉐이크 효과를 실행합니다
    /// </summary>
    public void Shake(float amount = 0.5f)
    {
        shakeAmount = amount;
    }

    /// <summary>
    /// 맵 경계를 설정합니다
    /// </summary>
    public void SetMapBoundaries(float minX, float maxX, float minY, float maxY)
    {
        mapMinX = minX;
        mapMaxX = maxX;
        mapMinY = minY;
        mapMaxY = maxY;
    }
}
