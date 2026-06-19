using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 5f;            // 걷기 속도
        public float runSpeed = 10f;            // 달리기 속도

        [Header("Jump")]
        public float jumpForce = 10f;           // 점프 초기 힘
        public float jumpHoldMultiplier = 2f;   // 버튼 누르고 있을 때 추가 중력 감소 배율
        public float fallMultiplier = 2.5f;     // 낙하 시 중력 배율 (클수록 빠르게 떨어짐)
        public float lowJumpMultiplier = 2f;    // 버튼 일찍 뗐을 때 중력 배율

        [Header("Ground Check")]
        public Transform groundCheck;           // 발 위치 트랜스폼 (빈 오브젝트로 발 위치에 배치)
        public float groundCheckRadius = 0.1f;  // 땅 감지 반경
        public LayerMask groundLayer;           // 땅으로 인식할 레이어

        private bool isMove = false;
        private bool isRun = false;
        private bool isJump = false;
        private bool isFall = false;
        private bool isGrounded = false;        // 현재 땅에 있는지
        private bool jumpButtonHeld = false;    // 점프 버튼 누르고 있는지

        private Rigidbody2D rb;
        private Animator animator;
        private Vector2 moveInput = Vector2.zero;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            // 원형으로 땅 감지 (groundCheck 위치에서 groundLayer에 닿으면 true)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // 낙하 중인지 판단 (Y속도가 음수면 낙하)
            bool falling = rb.linearVelocity.y < -0.1f;

            // 착지 감지: 이전에 점프/낙하 중이었는데 땅에 닿으면
            if (isGrounded && (isJump || isFall))
            {
                isJump = false;
                isFall = false;
                animator.SetBool(AnimationString.JumpTrigger, false);
                animator.SetBool(AnimationString.isFall, false);
            }

            // 낙하 애니메이션: 점프 중에 내려오기 시작하면
            if (isJump && falling && !isFall)
            {
                isFall = true;
                animator.SetBool(AnimationString.isFall, true);
            }
        }

        void FixedUpdate()
        {
            if (rb == null) return;

            // 좌우 이동 (달리기 여부에 따라 속도 다름)
            float currentSpeed = isRun ? runSpeed : moveSpeed;
            rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);

            // 점프 높이 조절: 버튼을 누르고 있으면 더 높이 올라감
            // 버튼을 일찍 떼면 중력을 더 강하게 적용해서 낮게 점프
            if (rb.linearVelocity.y > 0 && jumpButtonHeld)
            {
                // 버튼 누르고 있는 동안 중력 약하게 (더 높이 올라감)
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (jumpHoldMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !jumpButtonHeld)
            {
                // 버튼 일찍 뗐으면 중력 강하게 (낮게 점프)
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (rb.linearVelocity.y < 0)
            {
                // 낙하 시 중력 강하게 (더 빠르게 떨어짐)
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
        }

        // Input Action의 Move 이벤트에 연결
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            isMove = moveInput != Vector2.zero;
            animator.SetBool(AnimationString.isMove, isMove);

            if (moveInput.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (moveInput.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
        }

        // Input Action의 Run 이벤트에 연결 (Shift 키)
        public void OnRun(InputAction.CallbackContext context)
        {
            isRun = context.ReadValueAsButton();
            animator.SetBool(AnimationString.isRun, isRun);
        }

        // Input Action의 Jump 이벤트에 연결 (Space 키)
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && isGrounded)
            {
                // 점프 시작: 위로 힘 주기

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isJump = true;
                jumpButtonHeld = true;
                animator.SetBool(AnimationString.isJump, true);
            }

            if (context.canceled)
            {
                // 버튼 뗌: 낮은 점프로 전환
                jumpButtonHeld = false;
            }
        }

        // 땅 감지 범위를 씬 뷰에서 시각적으로 확인할 수 있게 그려줌
        void OnDrawGizmosSelected()
        {
            if (groundCheck == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}