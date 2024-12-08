using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryBehavior : MonoBehaviour
{
    public GameObject applePrefab; // ��� ������ ����
    private bool hasCollided = false;   // �浹 ���� Ȯ��

    void OnCollisionEnter(Collision collision)
    {
        // �̹� �浹 ó�� ���̸� ����
        if (hasCollided) return;

        // �浹�� ������Ʈ�� �������� Ȯ��
        if (collision.gameObject.CompareTag("Strawberry"))
        {
            // �浹�� ������Ʈ���� �ߺ� ó���� ����
            StrawberryBehavior otherStrawberry = collision.gameObject.GetComponent<StrawberryBehavior>();
            if (otherStrawberry != null && otherStrawberry.hasCollided) return;

            // �浹 ó�� ����
            hasCollided = true;

            // �浹 ���� ���
            Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;

            // ���� ������Ʈ ����
            Instantiate(applePrefab, spawnPosition, Quaternion.identity);

            // �浹�� �� ���� ����
            Destroy(collision.gameObject); // �浹�� ���� ����
            Destroy(gameObject);          // �ڽ� ����
        }
    }
}
