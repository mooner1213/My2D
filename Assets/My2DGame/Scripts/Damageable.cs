using UnityEngine;
using UnityEngine.Events;

namespace My2DGame
{
    /// <summary>
    /// 캐릭터의 체력을 관리하는 클래스
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;

        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        // 무적 체크
        private bool isInvincible = false;

        [SerializeField] private float invincibleTimer = 0.5f;
        private float countdown = 0f;

        //죽음 체크
        private bool isDeath = false;

        // 데미지 입을 때 등록된 함수를 호출하는 이벤트 함수 정의
        public UnityAction<float, Vector2> hitAction;
        #endregion

        #region Property
        public float MaxHealth
        {
            get { return maxHealth; }
            private set { maxHealth = value; }
        }

        public float CurrentHealth
        {
            get { return currentHealth; }
            private set { currentHealth = value; }
        }

        public bool IsDeath
        {
            get { return isDeath; }  // 소문자 isDeath → 백킹 필드 참조
            private set
            {
                isDeath = value;
                animator.SetBool(AnimationString.isDeath, value);
            }
        }

        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            //초기화
            CurrentHealth = MaxHealth;
        }

        private void Update()
        {
            if (IsDeath) return;

            // 무적모드
            if (isInvincible)
            {
                countdown += Time.deltaTime;
                if(countdown > invincibleTimer)
                {
                    // 무적모드 해제
                    isInvincible = false;

                    // 타이머 초기화
                    countdown = 0f;
                }
            }
        }
        #endregion

        #region Custom Method
        public void TakeDamage(float damage, Vector2 knockback)
        {
            // 죽음 체크, 무적 모드 체크
            if (IsDeath || isInvincible ) return;
            
            CurrentHealth -= damage;
            Debug.Log($"CurrentHealth : {currentHealth}");

            // 무적모드
            isInvincible = true;
            countdown = 0f;

            // 애니메이션 처리
            animator.SetTrigger(AnimationString.hitTrigger);

            // 데미지 효과 : vfx, sfx

            // 데미지 처리 : 넉백, UI
            if(hitAction != null)
            {
                hitAction.Invoke(damage, knockback);
            }

            if(CurrentHealth <= 0f)
            {
                Death();
            }
        }

        void Death()
        {
            IsDeath = true;  // 소문자 → 대문자로, setter 통해야 애니메이션 작동
        }
        #endregion
    }
}