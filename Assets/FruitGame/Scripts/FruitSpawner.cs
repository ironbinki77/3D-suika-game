using System.Collections;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject fruitPrefab;   // 생성할 과일 프리팹
    private GameObject currentFruit; // 현재 생성된 과일
    public float respawnDelay = 10f; // 과일 재생성 대기 시간

    private bool isRespawning = false; // 재생성 중인지 여부

    void Start()
    {
        // 게임 시작 시 과일 생성
        currentFruit = Instantiate(fruitPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        // 현재 과일이 없고, 재생성 중이 아니면 과일 생성
        if (currentFruit == null && !isRespawning)
        {
            StartCoroutine(RespawnFruit());
        }
    }

    private IEnumerator RespawnFruit()
    {
        isRespawning = true;

        // 재생성 대기 시간
        yield return new WaitForSeconds(respawnDelay);

        // 과일 생성
        currentFruit = Instantiate(fruitPrefab, transform.position, Quaternion.identity);
        isRespawning = false;
    }
}
