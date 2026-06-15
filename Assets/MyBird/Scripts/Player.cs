using UnityEngine;

namespace MyBird
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public enum PlayerState { Idle, Playing, Dead }

        [Header("Controls")]
        public KeyCode flapKey = KeyCode.Space;

        [Header("Movement")]
        public float forwardSpeed = 2f;
        public float flapForce = 5f;

        [Header("Idle Hover")]
        public float idleXOffset = -3f;      // 카메라 기준 X 오프셋 (왼쪽 위치 고정)
        public float idleYOffset = 0f;       // 카메라 기준 Y 오프셋 (높이 고정)

        [Header("Idle Gravity")]
        public bool overrideIdleGravity = false; // true면 아래 값으로 idle 중 gravity를 설정
        public float idleGravityScale = 0f;      // idle 중에 적용할 gravityScale (기본 0)

        [Header("Play Gravity")]
        public float playGravityScale = 1f;      // 플레이 중 적용할 gravityScale (조정 가능)

        [Header("Rotation")]
        public float rotationMultiplier = 5f;
        public float maxRotation = 45f;
        public float minRotation = -90f;
        public float rotationSmooth = 8f;

        [HideInInspector] public PlayerState State = PlayerState.Idle;

        Rigidbody2D rb;
        float originalGravityScale;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            originalGravityScale = rb.gravityScale;
            State = PlayerState.Idle;
            // 권장: Inspector에서 Rigidbody2D.Interpolate = Interpolate로 설정
        }

        void FixedUpdate()
        {
            if (State == PlayerState.Playing)
            {
                // 플레이 시작 시 gravity 복원/설정
                if (rb.gravityScale != playGravityScale) rb.gravityScale = playGravityScale;
                rb.linearVelocity = new Vector2(forwardSpeed, rb.linearVelocity.y);
                return;
            }

            if (State == PlayerState.Idle)
            {
                // Idle일 때 gravity 설정: Inspector에서 제어 가능
                float targetGravity = overrideIdleGravity ? idleGravityScale : 0f;
                if (rb.gravityScale != targetGravity) rb.gravityScale = targetGravity;

                // 카메라 기준으로 X/Y 위치를 강제 고정하여 미세한 떨어짐 방지
                var cam = Camera.main;
                if (cam != null)
                {
                    float targetX = cam.transform.position.x + idleXOffset;
                    float targetY = cam.transform.position.y + idleYOffset;
                    rb.position = new Vector2(targetX, targetY);
                    rb.linearVelocity = Vector2.zero; // 속도 완전 고정
                }
            }
        }

        void Update()
        {
            bool inputDown =
                Input.GetKeyDown(flapKey) ||
                Input.GetMouseButtonDown(0) ||
                (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

            if (State == PlayerState.Idle && inputDown)
            {
                StartPlaying();
                return;
            }

            if (State == PlayerState.Playing && inputDown)
            {
                rb.linearVelocity = new Vector2(forwardSpeed, flapForce);
            }

            ApplyRotation();
        }

        void StartPlaying()
        {
            State = PlayerState.Playing;
            rb.gravityScale = playGravityScale;
            rb.linearVelocity = new Vector2(forwardSpeed, flapForce);
        }

        void ApplyRotation()
        {
            float targetAngle = Mathf.Clamp(rb.linearVelocity.y * rotationMultiplier, minRotation, maxRotation);
            float currentZ = transform.eulerAngles.z;
            if (currentZ > 180f) currentZ -= 360f;
            float smoothedZ = Mathf.Lerp(currentZ, targetAngle, rotationSmooth * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, smoothedZ);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (State == PlayerState.Dead) return;

            // 파이프에 부딪히면 게임 정지(게임오버) + 플레이어가 추락하도록 gravity 복원
            if (collision.collider.CompareTag("Pipe"))
            {
                State = PlayerState.Dead;
                // 플레이어가 추락하도록 gravity 복원 (Inspector의 playGravityScale 사용)
                rb.gravityScale = playGravityScale != 0f ? playGravityScale : originalGravityScale;
                // 원하면 약간의 아래방향 관성 추가 (선택적)
                rb.linearVelocity = new Vector2(0f, Mathf.Min(rb.linearVelocity.y, -1f));

                if (GameManager.Instance != null) GameManager.Instance.GameOver();
                return;
            }

            // 바닥 등 다른 충돌은 기존처럼 처리(멈춤)
            if (collision.collider.CompareTag("Ground"))
            {
                State = PlayerState.Dead;
                rb.linearVelocity = Vector2.zero;
                rb.simulated = false;

                if (GameManager.Instance != null) GameManager.Instance.GameOver();
            }
        }
    }
}