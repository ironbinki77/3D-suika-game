using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ ���ӽ����̽� �߰�
using System.Collections; // �ڷ�ƾ(IEnumerator) ����� ���� ���ӽ����̽�
using UnityEngine.UI; // UI ����� ���� ���ӽ����̽�

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;        // ĳ���� �̵� �ӵ�
    public float jumpForce = 7f;       // ���� ��
    private Rigidbody rb;              // Rigidbody ����
    private bool isGrounded;           // ���� ��� �ִ��� ����
    private Vector3 moveDirection;     // �̵� ���� ����
    public enum PlayerState { Cherry, Strawberry, Apple, Orange, Watermelon }
    public PlayerState currentState = PlayerState.Cherry; // �ʱ� ����: ü��
    public GameObject watermelonModel; // ���� �� (��Ȱ��ȭ ���·� �غ�)
    public GameObject orangeModel;     // ������ �� (��Ȱ��ȭ ���·� �غ�)
    public GameObject appleModel;      // ��� �� (��Ȱ��ȭ ���·� �غ�)
    public GameObject strawberryModel; // ���� �� (��Ȱ��ȭ ���·� �غ�)
    public GameObject cherryModel;     // ü�� �� (�⺻ ��)
    private Collider playerCollider;   // Player�� Collider ����
    public Slider timerBar;            // �߾� ��� Ÿ�̸� ��
    public Image timerBarFill;      // Ÿ�̸� ���� Fill �̹��� (���� �����)
    public Image timerBarHandle;    // Ÿ�̸� ���� �ڵ� �̹��� (���� �̹��� �����)
    private float countdown = 0f;      // Ÿ�̸� ī��Ʈ�ٿ�
    private bool isTimerActive = false; // Ÿ�̸� Ȱ��ȭ ����
    public GameObject warningOverlay; // ȭ�� ������ ȿ���� ���� ��������
    private bool isWarningActive = false; // ��� ���� ����

    private readonly Color strawberryColor = new Color(1f, 0.4f, 0.4f); // ���� ������
    private readonly Color appleColor = Color.red;
    private readonly Color orangeColor = new Color(1f, 0.65f, 0f); // ��Ȳ��

    public Sprite strawberrySprite; // ���� �ڵ� �̹���
    public Sprite appleSprite;      // ��� �ڵ� �̹���
    public Sprite orangeSprite;     // ������ �ڵ� �̹���

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // ���� ���� Ȱ��ȭ

        // �ʱ� ���� ���� (ü��)
        UpdatePlayerModel();

        // Ÿ�̸� ��Ȱ��ȭ �ʱ�ȭ
        if (timerBar != null)
        {
            timerBar.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // ĳ���Ͱ� Y ��ǥ -10 ���Ϸ� �������� ���� ó��
        if (transform.position.y < -10)
        {
            StartCoroutine(RestartSceneWithDelay(1f)); // 1�� ������ �� �����
        }

        // ����Ű �Է� �޾ƿ���
        float horizontal = Input.GetAxis("Horizontal"); // A, D �Ǵ� �¿� ȭ��ǥ
        float vertical = Input.GetAxis("Vertical");     // W, S �Ǵ� ���� ȭ��ǥ

        // ī�޶��� ���� �������� �̵� ���� ���
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // ī�޶��� ���� ���⿡�� Y�� ���� (���� �̵��� ���)
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // ī�޶� ���⿡ ���� �Է� ��ȯ
        moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        // �����̽��ٸ� ���� ����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // ���� �Ŀ��� ���� ��� ���� ����
        }

        // ĳ���� �̵� ó��
        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 velocity = moveDirection * moveSpeed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z); // �̵� �ӵ� ����
        }
        else
        {
            // Ű �Է��� ������ �ӵ��� 0���� ����
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        // Ÿ�̸� ������Ʈ
        if (isTimerActive && countdown > 0f)
        {
            countdown -= Time.deltaTime;
            if (timerBar != null)
            {
                timerBar.value = countdown;
            }

            // 1.5�� ���Ϸ� ������ �� ��� ����
            if (countdown <= 1.5f && !isWarningActive)
            {
                StartCoroutine(StartWarningEffect());
            }

            // Ÿ�̸� ���� �� ���� ���·� ��ȯ
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

            float blinkDuration = 0.7f; // �����̴� ����
            while (countdown > 0f && countdown <= 1.5f)
            {
                // Alpha ���� 0���� 1�� �ݺ�
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

            warningOverlay.SetActive(false); // Ÿ�̸Ӱ� ����Ǹ� �������� ��Ȱ��ȭ
        }

        isWarningActive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �ڽŰ� ���� �𵨰� �浹�ߴ��� Ȯ��
        if (collision.gameObject.CompareTag(currentState.ToString()))
        {
            Destroy(collision.gameObject);
            AdvanceState(); // ���� ���·� ��ȯ
        }

        // ��ֹ��� �ε����� ���� ó��
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(RestartSceneWithDelay(1f)); // 1�� ������ �� �����
        }

        // �ٴڿ� ��Ҵ��� Ȯ��
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    private IEnumerator RestartSceneWithDelay(float delay)
    {
        // ȭ�� ���̵� �ƿ�
        FadeManager fadeManager = FindObjectOfType<FadeManager>();
        if (fadeManager != null)
        {
            yield return fadeManager.FadeOutAndWait(delay); // ���̵� �ƿ� �Ϸ���� ���
        }
        else
        {
            Debug.LogWarning("FadeManager not found. Proceeding with scene restart without fade.");
            yield return new WaitForSecondsRealtime(delay); // ���ο� ��� ������ ���� �ʵ��� Realtime ���
        }

        // �� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void AdvanceState()
    {
        // PlayerState�� ��ȯ������ ����
        currentState = (PlayerState)(((int)currentState + 1) % System.Enum.GetValues(typeof(PlayerState)).Length);
        UpdatePlayerModel();

        // Ÿ�̸� Ȱ��ȭ ����: ����, ���, ������ ����
        if (currentState == PlayerState.Strawberry || currentState == PlayerState.Apple || currentState == PlayerState.Orange)
        {
            ActivateTimer(10f); // 10�� Ÿ�̸� ����
        }
        else
        {
            DeactivateTimer(); // Ÿ�̸� ��Ȱ��ȭ
        }

        // ���� ���°� �Ǹ� 1�� �� �¸� ó��
        if (currentState == PlayerState.Watermelon)
        {
            CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();
            if (cameraFollow != null)
            {
                // ���ο� offset ������ 3�� ���� ����
                cameraFollow.IncreaseOffsetOverTime(new Vector3(0f, 10f, -25f), 3f);
            }
            StartCoroutine(DelayedWin());
        }
    }

    private IEnumerator DelayedWin()
    {
        yield return new WaitForSeconds(3f); // 3�� ���

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.HandleWin();
        }
    }

    private void RevertToPreviousState()
    {
        // ���� ���·� �ǵ�����
        currentState = (PlayerState)(((int)currentState - 1 + System.Enum.GetValues(typeof(PlayerState)).Length) % System.Enum.GetValues(typeof(PlayerState)).Length);
        UpdatePlayerModel();

        // Ÿ�̸� Ȱ��ȭ ����: ����, ���, ������ ����
        if (currentState == PlayerState.Strawberry || currentState == PlayerState.Apple)
        {
            ActivateTimer(10f); // 10�� Ÿ�̸� ����
        }
        else
        {
            DeactivateTimer(); // Ÿ�̸� ��Ȱ��ȭ
        }

        if (warningOverlay != null)
        {
            warningOverlay.SetActive(false); // ��� ȿ�� �ߴ�
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
            timerBar.gameObject.SetActive(true); // Ÿ�̸� UI Ȱ��ȭ
        }

        // ���º� Fill ���� �� �ڵ� �̹��� ����
        if (timerBarFill != null && timerBarHandle != null)
        {
            switch (currentState)
            {
                case PlayerState.Strawberry:
                    timerBarFill.color = strawberryColor;
                    timerBarHandle.sprite = strawberrySprite; // ���� �̹��� ����
                    break;
                case PlayerState.Apple:
                    timerBarFill.color = appleColor;
                    timerBarHandle.sprite = appleSprite; // ��� �̹��� ����
                    break;
                case PlayerState.Orange:
                    timerBarFill.color = orangeColor;
                    timerBarHandle.sprite = orangeSprite; // ������ �̹��� ����
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
            timerBar.gameObject.SetActive(false); // Ÿ�̸� UI ��Ȱ��ȭ
        }

        if (warningOverlay != null)
        {
            warningOverlay.SetActive(false); // ��� ȿ�� �ߴ�
        }
    }

    private void UpdatePlayerModel()
    {
        // ��� �� ��Ȱ��ȭ
        cherryModel.SetActive(false);
        strawberryModel.SetActive(false);
        appleModel.SetActive(false);
        orangeModel.SetActive(false);
        watermelonModel.SetActive(false);

        // ���� ���¿� �ش��ϴ� �� Ȱ��ȭ
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
