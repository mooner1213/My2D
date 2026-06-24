using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// HotBox에 부착되어 Damageable 충돌체에게 데미지를 주는 클래스
    /// </summary>
    public class Attack : MonoBehaviour
    {
        #region Variables
        // 공격력
        [SerializeField] private float attackDamage = 10f;

        // 넉백 효과
        [SerializeField] private Vector2 knockback = Vector2.zero;

        // 공격효과 처리
        public GameObject damageEffectPrefeb;
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Damageable damageable = collision.GetComponent<Damageable>();

            if (damageable != null)
            {
                // 넉백 방향 설정
                Transform directionTrans = transform.parent != null ? transform.parent : this.transform;
                Vector2 deliveredKnockback = directionTrans.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                damageable.TakeDamage(attackDamage, deliveredKnockback);

                // 데미지 효과 - vfx, sfx 리소스 있으면 효과 처리
                if (damageEffectPrefeb != null)
                {
                    GameObject effectGo = Instantiate(damageEffectPrefeb, transform.position, Quaternion.identity);
                    Destroy(effectGo, 1f);
                }

            }
        }
        #endregion
        
    }
}