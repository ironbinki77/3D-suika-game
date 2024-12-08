using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryBehavior : MonoBehaviour
{
    public GameObject applePrefab; // 사과 프리팹 참조
    private bool hasCollided = false;   // 충돌 여부 확인

    void OnCollisionEnter(Collision collision)
    {
        // 이미 충돌 처리 중이면 무시
        if (hasCollided) return;

        // 충돌한 오브젝트가 딸기인지 확인
        if (collision.gameObject.CompareTag("Strawberry"))
        {
            // 충돌한 오브젝트에도 중복 처리를 방지
            StrawberryBehavior otherStrawberry = collision.gameObject.GetComponent<StrawberryBehavior>();
            if (otherStrawberry != null && otherStrawberry.hasCollided) return;

            // 충돌 처리 시작
            hasCollided = true;

            // 충돌 지점 계산
            Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;

            // 딸기 오브젝트 생성
            Instantiate(applePrefab, spawnPosition, Quaternion.identity);

            // 충돌한 두 딸기 제거
            Destroy(collision.gameObject); // 충돌한 딸기 제거
            Destroy(gameObject);          // 자신 제거
        }
    }
}
