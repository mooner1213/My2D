using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 배경의 패럴랙스 효과를 구현하는 클래스입니다.
    /// </summary>
    public class ParallaxEffect : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform playerTransform;

        [Header("Settings")]
        [SerializeField] private bool followX = true;
        [SerializeField] private bool followY = false;

        private Vector3 previousCameraPosition;
        private Vector3 initialPosition;

        private void Start()
        {
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            if (playerTransform == null)
            {
                // Try to find a component named "PlayerController" without requiring a compile-time reference.
                var behaviours = FindObjectsOfType<MonoBehaviour>();
                foreach (var b in behaviours)
                {
                    if (b != null && b.GetType().Name == "PlayerController")
                    {
                        playerTransform = b.transform;
                        break;
                    }
                }

                // Fallback: try find by tag "Player"
                if (playerTransform == null)
                {
                    var playerObj = GameObject.FindWithTag("Player");
                    if (playerObj != null)
                        playerTransform = playerObj.transform;
                }
            }

            previousCameraPosition = cameraTransform != null ? cameraTransform.position : Vector3.zero;
            initialPosition = transform.position;
        }

        private void LateUpdate()
        {
            if (cameraTransform == null || playerTransform == null) return;

            Vector3 camPos = cameraTransform.position;
            Vector3 camDelta = camPos - previousCameraPosition;

            float camToPlayer = Mathf.Abs(playerTransform.position.z - cameraTransform.position.z);
            float camToBackground = Mathf.Abs(transform.position.z - cameraTransform.position.z);

            float parallaxFactor = 1f;
            if (camToBackground > Mathf.Epsilon)
            {
                parallaxFactor = camToPlayer / camToBackground;
            }

            Vector3 newPos = transform.position;
            if (followX)
                newPos.x += camDelta.x * parallaxFactor;
            if (followY)
                newPos.y += camDelta.y * parallaxFactor;

            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

            previousCameraPosition = camPos;
        }
    }
}
