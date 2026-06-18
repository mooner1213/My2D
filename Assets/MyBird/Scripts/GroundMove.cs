using System.Linq;
using UnityEngine;

namespace MyBird
{
    public class GroundMove : MonoBehaviour
    {
        [Tooltip("왼쪽으로 움직이는 속도")]
        public float speed = 2f;

        [Tooltip("화면 왼쪽 밖으로 나갔을 때 재배치 여유값")]
        public float recycleOffset = 1f;

        [Tooltip("비워두면 이 오브젝트의 자식들을 자동으로 사용")]
        public Transform[] groundParts;

        float partWidth;    // 땅 조각 하나의 너비
        bool isStopped;     // 게임오버 시 이동 중단 플래그

        void Start()
        {
            // groundParts가 비어있으면 자식 오브젝트들을 자동으로 등록
            if (groundParts == null || groundParts.Length == 0)
            {
                groundParts = GetComponentsInChildren<Transform>()
                              .Where(t => t != transform)   // 자기 자신은 제외
                              .ToArray();
            }

            if (groundParts.Length == 0) return;

            // 첫 번째 조각 기준으로 너비 계산
            partWidth = GetWidth(groundParts[0]);

            // X 위치 기준으로 왼쪽부터 정렬 (재배치 로직에 필요)
            groundParts = groundParts.OrderBy(t => t.position.x).ToArray();
        }

        void OnEnable()
        {
            // 오브젝트가 활성화될 때 게임오버 이벤트 구독
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameOver += Stop;
        }

        void OnDisable()
        {
            // 오브젝트가 비활성화될 때 이벤트 구독 해제 (메모리 누수 방지)
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameOver -= Stop;
        }

        void Stop()
        {
            // 게임오버 이벤트가 오면 이동 중단
            isStopped = true;
        }

        void Update()
        {
            // 멈췄거나 조각이 없으면 아무것도 안 함
            if (isStopped || groundParts == null || groundParts.Length == 0) return;

            float delta = speed * Time.deltaTime;

            // 모든 조각을 왼쪽으로 이동
            foreach (var part in groundParts)
            {
                part.position += Vector3.left * delta;
            }

            var cam = Camera.main;
            if (cam == null) return;

            // 카메라 왼쪽 끝보다 더 나간 조각을 오른쪽 끝으로 재배치
            float recycleThreshold = cam.transform.position.x - partWidth * 0.5f - recycleOffset;
            float rightmostX = groundParts.Max(t => t.position.x);

            foreach (var part in groundParts)
            {
                // 조각의 오른쪽 끝이 임계값보다 왼쪽이면 재배치
                if (part.position.x + partWidth * 0.5f < recycleThreshold)
                {
                    part.position = new Vector3(rightmostX + partWidth, part.position.y, part.position.z);
                    rightmostX = part.position.x;   // 재배치 후 가장 오른쪽 갱신
                }
            }
        }

        float GetWidth(Transform t)
        {
            // SpriteRenderer → Collider2D 순서로 너비를 가져옴
            var sr = t.GetComponent<SpriteRenderer>();
            if (sr != null) return sr.bounds.size.x;

            var col = t.GetComponent<Collider2D>();
            if (col != null) return col.bounds.size.x;

            return 10f; // 둘 다 없으면 기본값
        }
    }
}