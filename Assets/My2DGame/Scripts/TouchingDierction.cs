using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 접촉면 Ground, Wall, Celling 체크하는 클래스
    /// </summary>
    public class TouchingDierction : MonoBehaviour
    {
        #region Variables
        // 참조
        private CapsuleCollider2D touchingCol;
        private Animator animator;

        // 접촉면 체크 - 접촉면과의 거리안에 있는 충돌체 체크
        [SerializeField] private float groundDistance = 0.05f;

        // 접촉면 조건 설정
        [SerializeField] private ContactFilter2D contactFilter;

        // 레이를 쏘아 조건에 맞는 hit를 5개 가져온다.
        private RaycastHit2D[] groundHits = new RaycastHit2D[5];

        // 그라운드 체크
        private bool isGrounded;
        #endregion

        #region Property

        public bool IsGround
        {
            get
            {
                return IsGround;
            }
            private set
            {
                IsGround = value;
                animator.SetBool("isGrounded",value);
            }
        }

        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            // 참조
            touchingCol = this.GetComponent<CapsuleCollider2D>();
            animator = this.GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            IsGround = touchingCol.Cast(Vector2.down, contactFilter, groundHits, groundDistance) > 0;
        }
        #endregion
    }
}