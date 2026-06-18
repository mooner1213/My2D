using UnityEngine;

namespace MyBird
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;            // 따라갈 대상 (보통 플레이어)
        public float smoothTime = 0.12f;    // 기본 추적 부드러움 (클수록 느림)
        public float catchUpTime = 0.06f;   // 데드존 이탈 시 빠른 추적 시간
        public float xOffset = 0f;          // 타겟 기준 X 오프셋
        public float rightDeadZone = 1.0f;  // 플레이어가 이 범위 안에 있으면 카메라 안 움직임

        float fixedY;       // Y축은 고정 (2D 횡스크롤이라 위아래 안 움직임)
        float fixedZ;       // Z축도 고정
        float velocityX;    // SmoothDamp 내부에서 사용하는 속도값 (직접 건드리지 말 것)

        void Awake()
        {
            // 시작 시 Y, Z 위치를 저장해두고 계속 고정
            fixedY = transform.position.y;
            fixedZ = transform.position.z;

            // Inspector에서 target을 안 꽂았으면 자동으로 Player 탐색
            if (target == null)
            {
                var p = FindObjectOfType<Player>();
                if (p != null) target = p.transform;
            }
        }

        void LateUpdate()
        {
            // LateUpdate: 모든 Update가 끝난 후 실행 → 플레이어 이동 후 카메라 이동
            if (target == null) return;

            float camX = transform.position.x;
            float playerX = target.position.x + xOffset;

            // 기본은 현재 위치 유지 (데드존 안이면 안 움직임)
            float desiredX = camX;

            // 플레이어가 데드존 오른쪽을 벗어나면 카메라를 오른쪽으로 밀기
            if (playerX > camX + rightDeadZone)
            {
                desiredX = playerX - rightDeadZone;
            }

            // 플레이어가 얼마나 데드존을 벗어났는지에 따라 추적 속도 조절
            // 많이 벗어날수록 catchUpTime(빠름)에 가깝고, 조금 벗어나면 smoothTime(느림)
            float distance = Mathf.Max(0f, desiredX - camX);
            float usedSmooth = smoothTime;

            if (distance > 0.01f)
            {
                // t = 0이면 catchUpTime, t = 1이면 smoothTime
                float t = Mathf.Clamp01(distance / 5f);
                usedSmooth = Mathf.Lerp(catchUpTime, smoothTime, 1f - t);
            }

            // SmoothDamp: 부드럽게 desiredX로 이동 (velocityX는 내부 속도 상태)
            float newX = Mathf.SmoothDamp(camX, desiredX, ref velocityX, usedSmooth);

            // 카메라는 절대 왼쪽으로 되돌아가지 않음 (플래피버드 특성)
            newX = Mathf.Max(newX, camX);

            transform.position = new Vector3(newX, fixedY, fixedZ);
        }
    }
}