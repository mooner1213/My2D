using System.Linq;
using UnityEngine;

namespace MyBird
{
    public class GroundMove : MonoBehaviour
    {
        [Tooltip("왼쪽으로 움직이는 속도")]
        public float speed = 2f;

        [Tooltip("화면 왼쪽 밖으로 벗어났을 때 재배치하는 추가 여유값")]
        public float recycleOffset = 1f;

        [Tooltip("지정하지 않으면 이 오브젝트의 자식들을 그라운드 조각으로 사용합니다.")]
        public Transform[] groundParts;

        float partWidth = 0f;
        bool isStopped = false;

        void Start()
        {
            if (groundParts == null || groundParts.Length == 0)
            {
                var list = new System.Collections.Generic.List<Transform>();
                foreach (Transform t in transform) list.Add(t);
                groundParts = list.ToArray();
            }

            if (groundParts.Length == 0) return;

            partWidth = GetWidth(groundParts[0]);
            groundParts = groundParts.OrderBy(t => t.position.x).ToArray();

            // GameManager가 있으면 이벤트 구독
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver += HandleGameOver;
                if (GameManager.Instance.IsGameOver) HandleGameOver(); // 이미 게임오버면 즉시 처리
            }
        }

        void OnEnable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameOver += HandleGameOver;
        }

        void OnDisable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameOver -= HandleGameOver;
        }

        void HandleGameOver()
        {
            isStopped = true;
        }

        void Update()
        {
            if (groundParts == null || groundParts.Length == 0) return;
            if (isStopped) return;
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            {
                // 안전망: 이벤트를 놓쳤을 경우 즉시 정지
                isStopped = true;
                return;
            }

            float delta = speed * Time.deltaTime;

            foreach (var part in groundParts)
            {
                part.position += Vector3.left * delta;
            }

            var cam = Camera.main;
            if (cam == null) return;
            float camX = cam.transform.position.x;

            float recycleThreshold = camX - (partWidth * 0.5f) - recycleOffset;
            float rightmostX = groundParts.Max(t => t.position.x);

            for (int i = 0; i < groundParts.Length; i++)
            {
                var part = groundParts[i];
                if (part.position.x + (partWidth * 0.5f) < recycleThreshold)
                {
                    part.position = new Vector3(rightmostX + partWidth, part.position.y, part.position.z);
                    rightmostX = part.position.x;
                }
            }
        }

        float GetWidth(Transform t)
        {
            var sr = t.GetComponent<SpriteRenderer>();
            if (sr != null) return sr.bounds.size.x;

            var col = t.GetComponent<Collider2D>();
            if (col != null) return col.bounds.size.x;

            return 10f;
        }
    }
}