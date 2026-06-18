using UnityEngine;

namespace MyBird
{
    public class OffscreenDestroy : MonoBehaviour
    {
        [Tooltip("카메라 왼쪽 끝에서 추가로 여유를 둘 거리 (이 거리까지 나가야 삭제)")]
        public float offscreenPadding = 1f;

        void Update()
        {
            var cam = Camera.main;
            if (cam == null) return;

            // 카메라 왼쪽 끝 X좌표 계산
            // orthographicSize = 화면 세로 절반 크기
            // aspect = 가로/세로 비율 → 가로 절반 크기 계산에 사용
            float halfWidth = cam.orthographicSize * cam.aspect;
            float leftX = cam.transform.position.x - halfWidth - offscreenPadding;

            // 오브젝트가 카메라 왼쪽 끝을 벗어나면 삭제
            if (transform.position.x < leftX)
            {
                Destroy(gameObject);
            }
        }
    }
}