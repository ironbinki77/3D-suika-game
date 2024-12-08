using UnityEngine;
using System.Collections; // �ڷ�ƾ(IEnumerator) ����� ���� ���ӽ����̽�

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // ī�޶� ���� ��� (�÷��̾�)
    public Vector3 offset;           // ī�޶�� �÷��̾��� �Ÿ�
    public float smoothSpeed = 0.125f;
    public float rotationSpeed = 100f;
    public LayerMask collisionMask;  // ī�޶� �浹�� ������ ���̾�

    private float currentRotationY = 0f; // ���콺 X�� ȸ��
    private float currentRotationX = 0f; // ���콺 Y�� ȸ��
    private float minYAngle = -30f;      // Y�� ȸ�� �ּҰ�
    private float maxYAngle = 60f;       // Y�� ȸ�� �ִ밪
    private Quaternion currentRotation; // ���� ī�޶� ȸ�� ����
    private Vector3 currentVelocity;    // SmoothDamp �ӵ� ����

    void Update()
    {
        if (target == null) return; // ����� ������ ���� ����

        // ���콺 �Է� �޾� ȸ�� �� ������Ʈ
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        currentRotationY += mouseX;
        currentRotationX -= mouseY;

        // Y�� ȸ�� ���� ����
        currentRotationX = Mathf.Clamp(currentRotationX, minYAngle, maxYAngle);

        // ȸ�� �� ����
        currentRotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
    }

    void LateUpdate()
    {
        if (target == null) return; // ����� ������ ���� ����

        // ī�޶� ��ġ ���
        Vector3 desiredPosition = target.position + currentRotation * offset;

        // Raycasting���� �浹 ����
        Vector3 directionToCamera = desiredPosition - target.position;
        RaycastHit hit;

        // ī�޶�� Ÿ�� ������ �浹 ����
        if (Physics.Raycast(target.position, directionToCamera.normalized, out hit, directionToCamera.magnitude, collisionMask))
        {
            // �浹 ������ ī�޶� ��ġ�� ����
            desiredPosition = hit.point - directionToCamera.normalized * 0.1f; // �ణ�� ���� ���� �߰�
        }

        // �ε巴�� ��ġ �̵�
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
        transform.position = smoothedPosition;

        // ��� �ٶ󺸱�
        transform.LookAt(target.position + Vector3.up * offset.y);
    }

    // ������ ���ο� ĳ���͸� ���󰡵��� ����
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // ī�޶� offset�� ���������� ������Ű�� �ڷ�ƾ
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

        offset = newOffset; // ���� offset ����
    }
}
