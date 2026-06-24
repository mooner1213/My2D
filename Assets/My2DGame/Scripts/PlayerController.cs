using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    /// <summary>
    /// 플레이어 캐릭터의 움직임과 행동을 제어하는 클래스입니다.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody2D rb2D;
        private Animator animator;
        private TouchingDirection touchingDirection;
        private Damageable damageable;

        [Header("Movement")]        
        [SerializeField] private float walkSpeed = 4;
        [SerializeField] private float runSpeed = 8;
        private float moveSpeed;

        private Vector2 moveInput = Vector2.zero;

        //걷기 체크
        private bool isMove = false;
        //뛰기 체크
        private bool isRun = false;

        //반전
        private bool isFacingRight = true;

        //점프 - y축의 속도를 jumpForce 값으로 설정
        [SerializeField] private float jumpForce = 10f;
        #endregion

        #region Property
        public bool IsMove
        {
            get
            {
                return isMove; 
            }
            private set
            {
                isMove = value;
                animator.SetBool(AnimationString.isMove, value);
            }
        }

        public bool IsRun
        {
            get
            {
                return isRun;
            }
            private set
            {
                isRun = value;
                animator.SetBool(AnimationString.isRun, value);
            }
        }

        public bool IsFacingRight
        {
            get
            {
                return isFacingRight;
            }
            private set
            {
                //반전 체크
                if(isFacingRight != value)
                {
                    transform.localScale *= new Vector2(-1, 1);
                }
                isFacingRight = value;
            }
        }

        //이동 제어 - 애니메이터 파라미터 값 읽어오기
        public bool CannotMove
        {
            get
            {
                return animator.GetBool(AnimationString.cannotMove);
            }
        }

        // 속도 잠김 상태 읽어오기
        public bool LockVelocity
        {
            get
            {
                return animator.GetBool(AnimationString.lockVelocity);
            }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조 - 인스턴스 가져오기
            rb2D = GetComponent<Rigidbody2D>();
            if (rb2D == null)
            {
                Debug.LogError("PlayerController requires a Rigidbody2D component.");
            }
            
            animator = this.GetComponent<Animator>();
            touchingDirection = this.GetComponent<TouchingDirection>();
        }

        private void Start()
        {
            //초기화
        }

        private void FixedUpdate()
        {
            if (rb2D == null) return;

            if (LockVelocity == false)
            {
                //이동속도 얻어오기
                moveSpeed = GetCurrentMoveSpeed();

                // Rigidbody2D.velocity를 직접 설정하여 좌우 이동을 수행
                rb2D.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb2D.linearVelocity.y);

                //애니메이션 셋팅
                animator.SetFloat(AnimationString.yVelocity, rb2D.linearVelocity.y);
            }
        }
        #endregion

        #region Custom Method
        // New Input System -> Input Action "Move"에서 Invoke Unity Event 로 이 메서드를 연결하세요.
        // signature: Vector2 (x: 좌우 입력, y: 상하 입력) 를 받습니다.
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            //입력값에 따라 애니메이션 파라미터 제어
            IsMove = (moveInput != Vector2.zero);

            //바라보는 방향 전환
            SetFacingDirection(moveInput);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            //버튼 클릭 여부
            if(context.started) //버튼 down 시작
            {
                IsRun = true;
            }
            else if(context.canceled) //버튼 up 뗄때
            {
                IsRun = false;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            //버튼 클릭 여부
            if (context.started && touchingDirection.IsGround == true) //스페이스바 버튼 down 시작, 캐릭터가 그라운드에 있으면
            {
                animator.SetTrigger(AnimationString.jumpTrigger);
                rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
            }
        }


        public void OnAttack(InputAction.CallbackContext context)
        {
            //버튼 클릭 여부
            if (context.started && touchingDirection.IsGround == true) //마우스 왼클릭, 캐릭터가 그라운드에 있으면
            {
                animator.SetTrigger(AnimationString.attackTrigger);                
            }
        }


        //현재 이동 속도 구하기
        float GetCurrentMoveSpeed()
        {
            //이동제어 파라미터값이 true 또는 이동하지 않으면
            if(CannotMove == true || IsMove == false)
            {
                return 0f;
            }

            //걷기와 뛰기 구분
            if(IsRun)
            {
                return runSpeed;
            }
            else
            {
                return walkSpeed;
            }
        }

        //바라보는 방향 전환
        void SetFacingDirection(Vector2 moveInput)
        {
            if(moveInput.x > 0f && IsFacingRight == false)
            {
                IsFacingRight = true;
            }
            else if (moveInput.x < 0f && IsFacingRight == true)
            {
                IsFacingRight = false;
            }
        }

        // 데미지 이벤트 함수에 등록되는 함수
        void OnHit(float damage, Vector2 knockback)
        {
            // 넉백 값 적용
            rb2D.linearVelocity = new Vector2(knockback.x, rb2D.linearVelocity.y + knockback.y);
        }
        #endregion
    }
}
