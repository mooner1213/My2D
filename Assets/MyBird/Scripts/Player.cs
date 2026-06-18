using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyBird
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public enum PlayerState { Idle, Playing, Dead }

        [Header("Controls")]
        public KeyCode flapKey = KeyCode.Space;     // 날갯짓 키

        [Header("Movement")]
        public float forwardSpeed = 2f;             // 앞으로 이동 속도
        public float flapForce = 5f;                // 날갯짓 힘 (위로 튀는 세기)
        public float playGravityScale = 1f;         // 플레이 중 중력 배율

        [Header("Idle Position")]
        public float idleXOffset = -3f;             // 대기 중 카메라 기준 X 위치
        public float idleYOffset = 0f;              // 대기 중 카메라 기준 Y 위치

        [Header("Rotation")]
        public float rotationMultiplier = 5f;       // 속도 → 각도 변환 배율
        public float maxRotation = 45f;             // 최대 회전각 (위쪽)
        public float minRotation = -90f;            // 최소 회전각 (아래쪽)
        public float rotationSmooth = 8f;           // 회전 부드러움 (클수록 빠름)

        [HideInInspector] public PlayerState State = PlayerState.Idle;

        Rigidbody2D rb;
        Animator anim;  // 애니메이터 (없어도 null 체크로 에러 안 남)

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            rb.gravityScale = 0f;               // 대기 중엔 중력 없음
        }

        void FixedUpdate()
        {
            if (State == PlayerState.Playing)
            {
                // 앞으로 이동 속도를 매 프레임 고정 (Y축은 물리에 맡김)
                rb.linearVelocity = new Vector2(forwardSpeed, rb.linearVelocity.y);
                return;
            }

            if (State == PlayerState.Idle)
            {
                // 대기 중엔 카메라 기준 위치에 고정
                var cam = Camera.main;
                if (cam != null)
                {
                    rb.position = new Vector2(
                        cam.transform.position.x + idleXOffset,
                        cam.transform.position.y + idleYOffset
                    );
                }
                rb.linearVelocity = Vector2.zero;
            }
        }

        void Update()
        {
            if (State == PlayerState.Dead) return;

            // 스페이스 / 마우스 클릭 / 터치 중 하나라도 눌리면 true
            bool inputDown = Input.GetKeyDown(flapKey)
                          || Input.GetMouseButtonDown(0)
                          || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

            if (State == PlayerState.Idle && inputDown)
            {
                StartPlaying();
                return;
            }

            if (State == PlayerState.Playing && inputDown)
            {
                // 날갯짓: Y속도를 flapForce로 교체
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
            // Y속도를 각도로 변환 (올라갈 때 위로, 떨어질 때 아래로)
            float targetAngle = Mathf.Clamp(
                rb.linearVelocity.y * rotationMultiplier,
                minRotation,
                maxRotation
            );

            // eulerAngles는 0~360 반환 → 180 넘으면 음수로 변환해야 Lerp가 올바르게 동작
            float currentZ = transform.eulerAngles.z;
            if (currentZ > 180f) currentZ -= 360f;

            float smoothedZ = Mathf.Lerp(currentZ, targetAngle, rotationSmooth * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, smoothedZ);
        }

        void Die()
        {
            State = PlayerState.Dead;

            // 애니메이션을 현재 프레임에서 멈춤
            if (anim != null)
                anim.enabled = false;

            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (State == PlayerState.Dead) return;

            if (collision.collider.CompareTag("Pipe"))
            {
                // 앞으로 이동 멈추고 중력으로 뚝 떨어짐
                rb.linearVelocity = new Vector2(0f, Mathf.Min(rb.linearVelocity.y, -1f));
                Die();
                return;
            }

            if (collision.collider.CompareTag("Ground"))
            {
                rb.linearVelocity = Vector2.zero;
                rb.simulated = false;
                Die();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // Tag가 "Point"인 트리거를 통과하면 점수 +1
            // 파이프 루트의 IsTrigger BoxCollider2D가 Tag "Point"로 설정되어 있어야 함
            if (other.CompareTag("Point"))
            {
                if (ScoreManager.Instance != null)
                    ScoreManager.Instance.AddScore();
            }
        }
    }
}