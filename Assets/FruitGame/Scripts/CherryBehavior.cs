using UnityEngine;

public class CherryBehavior : MonoBehaviour
{
    public GameObject strawberryPrefab; // ���� ������ ����
    private bool hasCollided = false;   // �浹 ���� Ȯ��

    void OnCollisionEnter(Collision collision)
    {
        // �̹� �浹 ó�� ���̸� ����
        if (hasCollided) return;

        // �浹�� ������Ʈ�� ü������ Ȯ��
        if (collision.gameObject.CompareTag("Cherry"))
        {
            // �浹�� ������Ʈ���� �ߺ� ó���� ����
            CherryBehavior otherCherry = collision.gameObject.GetComponent<CherryBehavior>();
            if (otherCherry != null && otherCherry.hasCollided) return;

            // �浹 ó�� ����
            hasCollided = true;

            // �浹 ���� ���
            Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;

            // ���� ������Ʈ ����
            Instantiate(strawberryPrefab, spawnPosition, Quaternion.identity);

            // �浹�� �� ü�� ����
            Destroy(collision.gameObject); // �浹�� ü�� ����
            Destroy(gameObject);          // �ڽ� ����
        }
    }
}
