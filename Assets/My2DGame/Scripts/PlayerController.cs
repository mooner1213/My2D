using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [Tooltip("좌우 이동 속도")]
        public float moveSpeed = 5f;

        [Tooltip("이동 입력을 받을 때만 이동 허용")]
        public bool enableMovement = true;

        Rigidbody2D rb;
        Vector2 moveInput = Vector2.zero;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // New Input System: Move Action -> Invoke Unity Event(Vector2)
        // Input Action의 Invoke Unity Event에 이 함수를 연결하세요.
        public void OnMove(InputAction.CallbackContext context)
        {
            // 입력의 X값을 사용 (좌우)
            moveInput = context.ReadValue<Vector2>();
        }

        void FixedUpdate()
        {
            if (rb == null) return;
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        }
    }
}