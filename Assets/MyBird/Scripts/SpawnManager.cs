using System.Collections;
using UnityEngine;

namespace MyBird
{
    public class SpawnManager : MonoBehaviour
    {
        [Tooltip("파이프 프리팹 (상하 파이프가 묶인 프리팹)")]
        public GameObject pipePrefab;

        [Tooltip("스폰 위치 (X좌표를 기준으로 사용)")]
        public Transform spawnPoint;

        [Header("스폰 간격(초)")]
        public float spawnIntervalMin = 0.95f;
        public float spawnIntervalMax = 1.05f;

        [Header("스폰 Y 범위")]
        public float spawnYMin = -1.5f;
        public float spawnYMax = 3.5f;

        Player player;  // 플레이어 상태 확인용

        void Awake()
        {
            player = FindObjectOfType<Player>();
        }

        void OnEnable()
        {
            // 오브젝트가 활성화되면 스폰 루프 시작
            StartCoroutine(SpawnLoop());
        }

        void OnDisable()
        {
            // 오브젝트가 비활성화되면 모든 코루틴 중단
            StopAllCoroutines();
        }

        IEnumerator SpawnLoop()
        {
            // 첫 스폰까지 랜덤 대기 (게임 시작 직후 바로 나오는 것 방지)
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));

            while (true)
            {
                // 게임오버 상태면 루프 종료
                if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
                    yield break;

                // player가 null이면 다시 탐색 (혹시 씬 로드 직후 등의 상황 대비)
                if (player == null)
                    player = FindObjectOfType<Player>();

                // Playing 상태일 때만 파이프 스폰
                if (player != null && player.State == Player.PlayerState.Playing)
                    SpawnPipe();

                // 다음 스폰까지 랜덤 대기
                yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
            }
        }

        void SpawnPipe()
        {
            if (pipePrefab == null || spawnPoint == null) return;

            // Y위치를 랜덤으로 정해서 파이프 생성
            float y = Random.Range(spawnYMin, spawnYMax);
            Vector3 spawnPos = new Vector3(spawnPoint.position.x, y, spawnPoint.position.z);
            Instantiate(pipePrefab, spawnPos, Quaternion.identity);
        }
    }
}