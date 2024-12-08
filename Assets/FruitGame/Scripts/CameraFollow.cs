using UnityEngine;
using System.Collections; // 코루틴(IEnumerator) 사용을 위한 네임스페이스

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // 카메라가 따라갈 대상 (플레이어)
    public Vector3 offset;           // 카메라와 플레이어의 거리
    public float smoothSpeed = 0.125f;
    public float rotationSpeed = 100f;
    public LayerMask collisionMask;  // 카메라 충돌을 감지할 레이어

    private float currentRotationY = 0f; // 마우스 X축 회전
    private float currentRotationX = 0f; // 마우스 Y축 회전
    private float minYAngle = -30f;      // Y축 회전 최소값
    private float maxYAngle = 60f;       // Y축 회전 최대값
    private Quaternion currentRotation; // 현재 카메라 회전 저장
    private Vector3 currentVelocity;    // SmoothDamp 속도 저장

    void Update()
    {
        if (target == null) return; // 대상이 없으면 동작 멈춤

        // 마우스 입력 받아 회전 값 업데이트
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        currentRotationY += mouseX;
        currentRotationX -= mouseY;

        // Y축 회전 각도 제한
        currentRotationX = Mathf.Clamp(currentRotationX, minYAngle, maxYAngle);

        // 회전 값 저장
        currentRotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
    }

    void LateUpdate()
    {
        if (target == null) return; // 대상이 없으면 동작 멈춤

        // 카메라 위치 계산
        Vector3 desiredPosition = target.position + currentRotation * offset;

        // Raycasting으로 충돌 감지
        Vector3 directionToCamera = desiredPosition - target.position;
        RaycastHit hit;

        // 카메라와 타겟 사이의 충돌 감지
        if (Physics.Raycast(target.position, directionToCamera.normalized, out hit, directionToCamera.magnitude, collisionMask))
        {
            // 충돌 지점에 카메라 위치를 설정
            desiredPosition = hit.point - directionToCamera.normalized * 0.1f; // 약간의 여유 공간 추가
        }

        // 부드럽게 위치 이동
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
        transform.position = smoothedPosition;

        // 대상 바라보기
        transform.LookAt(target.position + Vector3.up * offset.y);
    }

    // 스폰된 새로운 캐릭터를 따라가도록 설정
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // 카메라 offset을 점진적으로 증가시키는 코루틴
    public void IncreaseOffsetOverTime(Vector3 newOffset, float duration)
    {
        StartCoroutine(AdjustOffset(newOffset, duration));
    }

    private IEnumerator AdjustOffset(Vector3 newOffset, float duration)
    {
        Vector3 initialOffset = offset;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            offset = Vector3.Lerp(initialOffset, newOffset, elapsedTime / duration);
            yield return null;
        }

        offset = newOffset; // 최종 offset 설정
    }
}
