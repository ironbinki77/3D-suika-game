using UnityEngine;

public class CherryBehavior : MonoBehaviour
{
    public GameObject strawberryPrefab; // 딸기 프리팹 참조
    private bool hasCollided = false;   // 충돌 여부 확인

    void OnCollisionEnter(Collision collision)
    {
        // 이미 충돌 처리 중이면 무시
        if (hasCollided) return;

        // 충돌한 오브젝트가 체리인지 확인
        if (collision.gameObject.CompareTag("Cherry"))
        {
            // 충돌한 오브젝트에도 중복 처리를 방지
            CherryBehavior otherCherry = collision.gameObject.GetComponent<CherryBehavior>();
            if (otherCherry != null && otherCherry.hasCollided) return;

            // 충돌 처리 시작
            hasCollided = true;

            // 충돌 지점 계산
            Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;

            // 딸기 오브젝트 생성
            Instantiate(strawberryPrefab, spawnPosition, Quaternion.identity);

            // 충돌한 두 체리 제거
            Destroy(collision.gameObject); // 충돌한 체리 제거
            Destroy(gameObject);          // 자신 제거
        }
    }
}
