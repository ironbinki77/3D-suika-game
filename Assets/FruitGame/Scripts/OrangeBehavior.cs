using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBehavior : MonoBehaviour
{
    public GameObject watermelonPrefab; // 오렌지 프리팹 참조
    private bool hasCollided = false;   // 충돌 여부 확인

    void OnCollisionEnter(Collision collision)
    {
        // 이미 충돌 처리 중이면 무시
        if (hasCollided) return;

        // 충돌한 오브젝트가 오렌지인지 확인
        if (collision.gameObject.CompareTag("Orange"))
        {
            // 충돌한 오브젝트에도 중복 처리를 방지
            OrangeBehavior otherOrange = collision.gameObject.GetComponent<OrangeBehavior>();
            if (otherOrange != null && otherOrange.hasCollided) return;

            // 충돌 처리 시작
            hasCollided = true;

            // 충돌 지점 계산
            Vector3 spawnPosition = (transform.position + collision.transform.position) / 2;

            // 오렌지 오브젝트 생성
            Instantiate(watermelonPrefab, spawnPosition, Quaternion.identity);

            // 충돌한 두 오렌지 제거
            Destroy(collision.gameObject); // 충돌한 오렌지 제거
            Destroy(gameObject);          // 자신 제거
        }
    }
}
