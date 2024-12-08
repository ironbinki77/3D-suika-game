using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리용 네임스페이스 추가
using System.Collections; // 코루틴(IEnumerator) 사용을 위한 네임스페이스
using UnityEngine.UI; // UI 사용을 위한 네임스페이스

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;        // 캐릭터 이동 속도
    public float jumpForce = 7f;       // 점프 힘
    private Rigidbody rb;              // Rigidbody 참조
    private bool isGrounded;           // 땅에 닿아 있는지 여부
    private Vector3 moveDirection;     // 이동 방향 저장
    public enum PlayerState { Cherry, Strawberry, Apple, Orange, Watermelon }
    public PlayerState currentState = PlayerState.Cherry; // 초기 상태: 체리
    public GameObject watermelonModel; // 수박 모델 (비활성화 상태로 준비)
    public GameObject orangeModel;     // 오렌지 모델 (비활성화 상태로 준비)
    public GameObject appleModel;      // 사과 모델 (비활성화 상태로 준비)
    public GameObject strawberryModel; // 딸기 모델 (비활성화 상태로 준비)
    public GameObject cherryModel;     // 체리 모델 (기본 모델)
    private Collider playerCollider;   // Player의 Collider 참조
    public Slider timerBar;            // 중앙 상단 타이머 바
    public Image timerBarFill;      // 타이머 바의 Fill 이미지 (색상 변경용)
    public Image timerBarHandle;    // 타이머 바의 핸들 이미지 (과일 이미지 변경용)
    private float countdown = 0f;      // 타이머 카운트다운
    private bool isTimerActive = false; // 타이머 활성화 여부
    public GameObject warningOverlay; // 화면 깜빡임 효과를 위한 오버레이
    private bool isWarningActive = false; // 경고 상태 여부

    private readonly Color strawberryColor = new Color(1f, 0.4f, 0.4f); // 연한 빨간색
    private readonly Color appleColor = Color.red;
    private readonly Color orangeColor = new Color(1f, 0.65f, 0f); // 주황색

    public Sprite strawberrySprite; // 딸기 핸들 이미지
    public Sprite appleSprite;      // 사과 핸들 이미지
    public Sprite orangeSprite;     // 오렌지 핸들 이미지

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // 물리 보간 활성화

        // 초기 상태 설정 (체리)
        UpdatePlayerModel();

        // 타이머 비활성화 초기화
        if (timerBar != null)
        {
            timerBar.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // 캐릭터가 Y 좌표 -10 이하로 떨어지면 죽음 처리
        if (transform.position.y < -10)
        {
            StartCoroutine(RestartSceneWithDelay(1f)); // 1초 딜레이 후 재시작
        }

        // 방향키 입력 받아오기
        float horizontal = Input.GetAxis("Horizontal"); // A, D 또는 좌우 화살표
        float vertical = Input.GetAxis("Vertical");     // W, S 또는 상하 화살표

        // 카메라의 방향 기준으로 이동 방향 계산
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // 카메라의 로컬 방향에서 Y축 제거 (수평 이동만 고려)
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // 카메라 방향에 따른 입력 변환
        moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        // 스페이스바를 눌러 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // 점프 후에는 땅에 닿아 있지 않음
        }

        // 캐릭터 이동 처리
        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 velocity = moveDirection * moveSpeed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z); // 이동 속도 설정
        }
        else
        {
            // 키 입력이 없으면 속도를 0으로 설정
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        // 타이머 업데이트
        if (isTimerActive && countdown > 0f)
        {
            countdown -= Time.deltaTime;
            if (timerBar != null)
            {
                timerBar.value = countdown;
            }

            // 1.5초 이하로 남았을 때 경고 시작
            if (countdown <= 1.5f && !isWarningActive)
            {
                StartCoroutine(StartWarningEffect());
            }

            // 타이머 종료 시 이전 상태로 전환
            if (countdown <= 0f)
            {
                RevertToPreviousState();
            }
        }
    }

    private IEnumerator StartWarningEffect()
    {
        isWarningActive = true;

        if (warningOverlay != null)
        {
            warningOverlay.SetActive(true);
            RawImage overlayImage = warningOverlay.GetComponent<RawImage>();

            float blinkDuration = 0.7f; // 깜빡이는 간격
            while (countdown > 0f && countdown <= 1.5f)
            {
                // Alpha 값을 0에서 1로 반복
                for (float alpha = 0f; alpha <= 0.4f; alpha += Time.deltaTime / blinkDuration)
                {
                    overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, alpha);
                    yield return null;
                }

                for (float alpha = 0.4f; alpha >= 0f; alpha -= Time.deltaTime / blinkDuration)
                {
                    overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, alpha);
                    yield return null;
                }
            }

            warningOverlay.SetActive(false); // 타이머가 종료되면 오버레이 비활성화
        }

        isWarningActive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 자신과 같은 모델과 충돌했는지 확인
        if (collision.gameObject.CompareTag(currentState.ToString()))
        {
            Destroy(collision.gameObject);
            AdvanceState(); // 다음 상태로 전환
        }

        // 장애물에 부딪히면 죽음 처리
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(RestartSceneWithDelay(1f)); // 1초 딜레이 후 재시작
        }

        // 바닥에 닿았는지 확인
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    private IEnumerator RestartSceneWithDelay(float delay)
    {
        // 화면 페이드 아웃
        FadeManager fadeManager = FindObjectOfType<FadeManager>();
        if (fadeManager != null)
        {
            yield return fadeManager.FadeOutAndWait(delay); // 페이드 아웃 완료까지 대기
        }
        else
        {
            Debug.LogWarning("FadeManager not found. Proceeding with scene restart without fade.");
            yield return new WaitForSecondsRealtime(delay); // 슬로우 모션 영향을 받지 않도록 Realtime 사용
        }

        // 씬 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void AdvanceState()
    {
        // PlayerState를 순환적으로 변경
        currentState = (PlayerState)(((int)currentState + 1) % System.Enum.GetValues(typeof(PlayerState)).Length);
        UpdatePlayerModel();

        // 타이머 활성화 조건: 딸기, 사과, 오렌지 상태
        if (currentState == PlayerState.Strawberry || currentState == PlayerState.Apple || currentState == PlayerState.Orange)
        {
            ActivateTimer(10f); // 10초 타이머 시작
        }
        else
        {
            DeactivateTimer(); // 타이머 비활성화
        }

        // 수박 상태가 되면 1초 후 승리 처리
        if (currentState == PlayerState.Watermelon)
        {
            CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();
            if (cameraFollow != null)
            {
                // 새로운 offset 값으로 3초 동안 변경
                cameraFollow.IncreaseOffsetOverTime(new Vector3(0f, 10f, -25f), 3f);
            }
            StartCoroutine(DelayedWin());
        }
    }

    private IEnumerator DelayedWin()
    {
        yield return new WaitForSeconds(3f); // 3초 대기

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.HandleWin();
        }
    }

    private void RevertToPreviousState()
    {
        // 이전 상태로 되돌리기
        currentState = (PlayerState)(((int)currentState - 1 + System.Enum.GetValues(typeof(PlayerState)).Length) % System.Enum.GetValues(typeof(PlayerState)).Length);
        UpdatePlayerModel();

        // 타이머 활성화 조건: 딸기, 사과, 오렌지 상태
        if (currentState == PlayerState.Strawberry || currentState == PlayerState.Apple)
        {
            ActivateTimer(10f); // 10초 타이머 시작
        }
        else
        {
            DeactivateTimer(); // 타이머 비활성화
        }

        if (warningOverlay != null)
        {
            warningOverlay.SetActive(false); // 경고 효과 중단
        }
    }

    private void ActivateTimer(float duration)
    {
        isTimerActive = true;
        countdown = duration;
        if (timerBar != null)
        {
            timerBar.maxValue = duration;
            timerBar.value = duration;
            timerBar.gameObject.SetActive(true); // 타이머 UI 활성화
        }

        // 상태별 Fill 색상 및 핸들 이미지 설정
        if (timerBarFill != null && timerBarHandle != null)
        {
            switch (currentState)
            {
                case PlayerState.Strawberry:
                    timerBarFill.color = strawberryColor;
                    timerBarHandle.sprite = strawberrySprite; // 딸기 이미지 설정
                    break;
                case PlayerState.Apple:
                    timerBarFill.color = appleColor;
                    timerBarHandle.sprite = appleSprite; // 사과 이미지 설정
                    break;
                case PlayerState.Orange:
                    timerBarFill.color = orangeColor;
                    timerBarHandle.sprite = orangeSprite; // 오렌지 이미지 설정
                    break;
            }
        }
    }

    private void DeactivateTimer()
    {
        isTimerActive = false;
        countdown = 0f;

        if (timerBar != null)
        {
            timerBar.gameObject.SetActive(false); // 타이머 UI 비활성화
        }

        if (warningOverlay != null)
        {
            warningOverlay.SetActive(false); // 경고 효과 중단
        }
    }

    private void UpdatePlayerModel()
    {
        // 모든 모델 비활성화
        cherryModel.SetActive(false);
        strawberryModel.SetActive(false);
        appleModel.SetActive(false);
        orangeModel.SetActive(false);
        watermelonModel.SetActive(false);

        // 현재 상태에 해당하는 모델 활성화
        switch (currentState)
        {
            case PlayerState.Cherry:
                cherryModel.SetActive(true);
                moveSpeed = 7f;
                jumpForce = 7f;
                break;

            case PlayerState.Strawberry:
                strawberryModel.SetActive(true);
                moveSpeed = 6f;
                jumpForce = 6.5f;
                break;

            case PlayerState.Apple:
                appleModel.SetActive(true);
                moveSpeed = 5.5f;
                jumpForce = 6f;
                break;

            case PlayerState.Orange:
                orangeModel.SetActive(true);
                moveSpeed = 5f;
                jumpForce = 5.5f;
                break;

            case PlayerState.Watermelon:
                watermelonModel.SetActive(true);
                moveSpeed = 0.1f;
                jumpForce = 0.1f;
                break;
        }
    }

}
