using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyBird
{
    public class GameManager : MonoBehaviour
    {
        // 싱글톤: 어디서든 GameManager.Instance로 접근 가능
        public static GameManager Instance { get; private set; }

        // 게임오버 여부 (외부에서 읽기만 가능)
        public bool IsGameOver { get; private set; }

        // 게임오버 시 발생하는 이벤트
        public event Action OnGameOver;

        void Awake()
        {
            // DontDestroyOnLoad 없이 씬마다 새로 생성
            // 같은 씬에 중복으로 있을 경우만 제거
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            // 이 GameManager가 Instance였다면 씬 종료 시 Instance를 비워줌
            // 다음 씬에서 새 GameManager가 Instance로 등록될 수 있게 함
            if (Instance == this)
                Instance = null;
        }

        public void GameOver()
        {
            // 중복 호출 방지
            if (IsGameOver) return;

            IsGameOver = true;

            // 구독 중인 모든 스크립트에게 게임오버 알림
            OnGameOver?.Invoke();
        }

        public void Restart()
        {
            // 현재 씬을 다시 로드 (모든 오브젝트 새로 생성됨)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}