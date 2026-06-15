using System.Collections;
using UnityEngine;

namespace MyBird
{
    public class SpawnManager : MonoBehaviour
    {
        [Tooltip("파이프 프리팹 (상단/하단 묶인 프리팹)")]
        public GameObject pipePrefab;

        [Tooltip("스폰 포인트(위치의 X를 사용)")]
        public Transform spawnPoint;

        [Header("스폰 간격(초)")]
        public float spawnIntervalMin = 0.95f;
        public float spawnIntervalMax = 1.05f;

        [Header("스폰 Y 범위")]
        public float spawnYMin = -1.5f;
        public float spawnYMax = 3.5f;

        Coroutine spawnLoop;
        Player player;

        void Awake()
        {
            player = FindObjectOfType<Player>();
        }

        void OnEnable()
        {
            StartSpawning();
        }

        void OnDisable()
        {
            StopSpawning();
        }

        public void StartSpawning()
        {
            if (spawnLoop == null) spawnLoop = StartCoroutine(SpawnLoop());
        }

        public void StopSpawning()
        {
            if (spawnLoop != null)
            {
                StopCoroutine(spawnLoop);
                spawnLoop = null;
            }
        }

        IEnumerator SpawnLoop()
        {
            // 첫 스폰까지 짧게 대기 (랜덤)
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));

            while (true)
            {
                // 게임오버 시 중단
                if (GameManager.Instance != null && GameManager.Instance.IsGameOver) yield break;

                // 플레이어가 Playing 상태일 때만 스폰
                if (player == null) player = FindObjectOfType<Player>();
                if (player != null && player.State == Player.PlayerState.Playing)
                {
                    SpawnPipe();
                }

                yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
            }
        }

        void SpawnPipe()
        {
            if (pipePrefab == null || spawnPoint == null) return;

            float y = Random.Range(spawnYMin, spawnYMax);
            Vector3 pos = new Vector3(spawnPoint.position.x, y, spawnPoint.position.z);
            Instantiate(pipePrefab, pos, Quaternion.identity);
        }
    }
}