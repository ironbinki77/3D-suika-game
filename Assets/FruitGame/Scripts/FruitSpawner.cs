using System.Collections;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject fruitPrefab;   // ������ ���� ������
    private GameObject currentFruit; // ���� ������ ����
    public float respawnDelay = 10f; // ���� ����� ��� �ð�

    private bool isRespawning = false; // ����� ������ ����

    void Start()
    {
        // ���� ���� �� ���� ����
        currentFruit = Instantiate(fruitPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        // ���� ������ ����, ����� ���� �ƴϸ� ���� ����
        if (currentFruit == null && !isRespawning)
        {
            StartCoroutine(RespawnFruit());
        }
    }

    private IEnumerator RespawnFruit()
    {
        isRespawning = true;

        // ����� ��� �ð�
        yield return new WaitForSeconds(respawnDelay);

        // ���� ����
        currentFruit = Instantiate(fruitPrefab, transform.position, Quaternion.identity);
        isRespawning = false;
    }
}
