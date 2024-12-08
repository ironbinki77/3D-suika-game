using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleBehavior : MonoBehaviour
{
    public GameObject orangePrefab; // ������ ������ ����
    private bool hasCollided = false;   // �浹 ���� Ȯ��

    void OnCollisionEnter(Collision collision)
    {
        // �̹� �浹 ó�� ���̸� ����
        if (hasCollided) return;

        // �浹�� ������Ʈ�� ������� Ȯ��
        if (collision.gameObject.CompareTag("Apple"))
        {
            // �浹�� ������Ʈ���� �ߺ� ó���� ����
            AppleBehavior otherApple = collision.gameObject.GetComponent<AppleBehavior>();
            if (otherApple != null && otherApple.hasCollided) return;

            // �浹 ó�� ����
            hasCollided = true;

            // �浹 ���� ���
            Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;

            // ������ ������Ʈ ����
            Instantiate(orangePrefab, spawnPosition, Quaternion.identity);

            // �浹�� �� ��� ����
            Destroy(collision.gameObject); // �浹�� ��� ����
            Destroy(gameObject);          // �ڽ� ����
        }
    }
}
