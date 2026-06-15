using UnityEngine;

namespace MyBird
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public float smoothTime = 0.12f;
        public float xOffset = 0f;

        // 오른쪽 데드존 크기
        public float rightDeadZone = 1.0f;

        // 데드존 초과 시 빠른 캐치업 속도 계수 (1 = 기존 SmoothDamp 동작, 작을수록 더 빠르게 따라옴)
        public float catchUpTime = 0.06f;

        float fixedY;
        float fixedZ;
        float velocityX;

        void Awake()
        {
            fixedY = transform.position.y;
            fixedZ = transform.position.z;

            if (target == null)
            {
                var p = FindObjectOfType<Player>();
                if (p != null) target = p.transform;
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            float camX = transform.position.x;
            float playerX = target.position.x + xOffset;

            float desiredX = camX;

            if (playerX > camX + rightDeadZone)
            {
                // 플레이어가 데드존을 벗어나면 데드존 경계로 설정
                desiredX = playerX - rightDeadZone;
            }

            // 데드존을 약간 넘었을 때는 원래 smoothTime, 많이 넘었을 때는 catchUpTime 사용
            float distance = Mathf.Max(0f, desiredX - camX);
            float usedSmoothTime = smoothTime;

            if (distance > 0.01f)
            {
                // 거리 비례로 더 빠르게(최소 catchUpTime까지)
                float t = Mathf.Clamp01(distance / 5f); // 5f는 가감 가능한 값
                usedSmoothTime = Mathf.Lerp(catchUpTime, smoothTime, 1f - t);
            }

            float newX = Mathf.SmoothDamp(camX, desiredX, ref velocityX, usedSmoothTime);

            // 절대 왼쪽으로는 이동하지 않음
            newX = Mathf.Max(newX, camX);

            transform.position = new Vector3(newX, fixedY, fixedZ);
        }
    }
}