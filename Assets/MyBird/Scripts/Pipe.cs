using UnityEngine;

namespace MyBird
{
    [RequireComponent(typeof(Transform))]
    public class OffscreenDestroy : MonoBehaviour
    {
        [Tooltip("카메라 왼쪽 바깥으로 나가면 삭제할 여유값")]
        public float offscreenPadding = 1f;

        void Update()
        {
            var cam = Camera.main;
            if (cam == null) return;

            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;
            float leftX = cam.transform.position.x - halfWidth - offscreenPadding;

            if (transform.position.x < leftX)
            {
                Destroy(gameObject);
            }
        }
    }
}