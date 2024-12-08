using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBehavior : MonoBehaviour
{
    public GameObject watermelonPrefab; // ������ ������ ����
    private bool hasCollided = false;   // �浹 ���� Ȯ��

    void OnCollisionEnter(Collision collision)
    {
        // �̹� �浹 ó�� ���̸� ����
        if (hasCollided) return;

        // �浹�� ������Ʈ�� ���������� Ȯ��
        if (collision.gameObject.CompareTag("Orange"))
        {
            // �浹�� ������Ʈ���� �ߺ� ó���� ����
            OrangeBehavior otherOrange = collision.gameObject.GetComponent<OrangeBehavior>();
            if (otherOrange != null && otherOrange.hasCollided) return;

            // �浹 ó�� ����
            hasCollided = true;

            // �浹 ���� ���
            Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;

            // ������ ������Ʈ ����
            Instantiate(watermelonPrefab, spawnPosition, Quaternion.identity);

            // �浹�� �� ������ ����
            Destroy(collision.gameObject); // �浹�� ������ ����
            Destroy(gameObject);          // �ڽ� ����
        }
    }
}
