using UnityEngine;

namespace My2DGame
{
    public class ParallaxEffect : MonoBehaviour
    {
        [Header("카메라 설정")]
        [Tooltip("메인 카메라의 Transform을 넣으세요. 비어있으면 자동으로 찾습니다.")]
        public Transform cameraTransform;

        [Header("패럴렉스 속도 설정 (0 ~ 1)")]
        [Range(0f, 1f)]
        [Tooltip("0이면 카메라와 똑같이 움직임(정지된 배경), 1이면 카메라가 움직여도 완벽히 고정됨(멀리 있는 배경)")]
        public float parallaxFactorX = 0.5f; // X축 움직임 조절 계수

        public bool useY = false;
        [Range(0f, 1f)]
        public float parallaxFactorY = 0.2f; // Y축 움직임 조절 계수

        // 기준이 될 카메라의 이전 위치를 기억하는 상자입니다.
        Vector3 prevCamPos;

        void Start()
        {
            // 만약 인스펙터에서 카메라를 안 넣어줬다면, 자동으로 메인 카메라를 찾아서 연결합니다.
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            // 게임 시작 시점의 카메라 위치를 저장합니다.
            if (cameraTransform != null)
                prevCamPos = cameraTransform.position;
        }

        void LateUpdate()
        {
            if (cameraTransform == null) return;

            // 1. 카메라가 지난 프레임에 비해 얼마나 움직였는지(이동량) 계산합니다.
            Vector3 camDelta = cameraTransform.position - prevCamPos;

            // 2. 배경이 이동해야 할 목표 위치를 계산합니다.
            // (카메라 이동량 * 패럴렉스 계수)만큼 배경을 현재 위치에서 더해줍니다.
            Vector3 targetPos = transform.position;

            if (parallaxFactorX > 0f) targetPos.x += camDelta.x * parallaxFactorX;
            if (useY && parallaxFactorY > 0f) targetPos.y += camDelta.y * parallaxFactorY;

            // 3. 계산된 부드러운 위치로 배경을 이동시킵니다.
            transform.position = targetPos;

            // 4. 다음 프레임 계산을 위해 현재 카메라 위치를 과거 위치로 저장합니다.
            prevCamPos = cameraTransform.position;
        }
    }
}