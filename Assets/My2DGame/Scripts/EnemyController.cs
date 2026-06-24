using UnityEngine;

namespace My2DGame
{
    public class EnemyController : MonoBehaviour
    {
        #region Variables
        private Rigidbody2D rb2D;
        private TouchingDirection touchingDirection;
        private Animator animator;
        private Damageable damageable;

        public DetectionZone detectionZone;
        public DetectionZone GetDetectionZone;

        [SerializeField] private float runSpeed = 5f;

        public enum WalkableDirection { Left, Right }
        private WalkableDirection walkDirection = WalkableDirection.Right;
        private Vector2 directionVector = Vector2.right;

        // HasTarget 백킹 필드
        private bool _hasTarget;

        public bool HasTarget
        {
            get { return _hasTarget; }  // 자기 자신 재귀 호출 제거
            private set
            {
                _hasTarget = value;
                animator.SetBool(AnimationString.hasTarget, value);  // animator = 제거
            }
        }

        public float CooldownTime
        {
            get { return animator.GetFloat(AnimationString.cooldownTime); }
            private set { animator.SetFloat(AnimationString.cooldownTime, value); }
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

        #region Property
        public WalkableDirection WalkDirection
        {
            get { return walkDirection; }
            private set
            {
                if (walkDirection != value)
                {
                    transform.localScale *= new Vector2(-1, 1);
                    directionVector = (value == WalkableDirection.Left) ? Vector2.left : Vector2.right;
                }
                walkDirection = value;
            }
        }

        // CannotMove는 여기 한 곳에만 선언
        public bool CannotMove
        {
            get { return animator.GetBool(AnimationString.cannotMove); }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            touchingDirection = GetComponent<TouchingDirection>();
            animator = GetComponent<Animator>();

            damageable = GetComponent<Damageable>();
            damageable.hitAction += OnHit;

            // 그라운드 디텍션 이벤트 함수 등록
            GetDetectionZone.noRemainColider += OnCliffDectection;
        }

        private void Update()
        {
            HasTarget = detectionZone.IsDectected;

            if (CooldownTime > 0f)
            {
                CooldownTime -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            if (touchingDirection.IsGround && touchingDirection.IsWall)
            {
                Flip();
            }

            // 이동 - 넉백효과동안 이동처리 안되게 해야함.
            if (LockVelocity == false)
            {
                if (CannotMove == false)
                {
                    rb2D.linearVelocity = new Vector2(directionVector.x * runSpeed, rb2D.linearVelocity.y);
                }
                else
                {
                    rb2D.linearVelocity = new Vector2(0f, rb2D.linearVelocity.y);  // .x → .y 오타도 수정
                }
                animator.SetBool("IsMove", true);
            }
        }
        #endregion

        #region Custom Method
        void Flip()
        {
            if (WalkDirection == WalkableDirection.Left)
                WalkDirection = WalkableDirection.Right;
            else if (WalkDirection == WalkableDirection.Right)
                WalkDirection = WalkableDirection.Left;
        }

        // 데미지 이벤트 함수에 등록되는 함수
        void OnHit (float damage, Vector2 knockback)
        {
            // 넉백 값 적용
            rb2D.linearVelocity = new Vector2(knockback.x, rb2D.linearVelocity.y + knockback.y);
        }

        // 낭떨어지 체크
        void OnCliffDectection()
        {
            if (touchingDirection.IsGround)
            {
                Flip();
            }
        }
        #endregion
    }
}