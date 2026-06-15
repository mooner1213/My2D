using System;
using UnityEngine;

namespace MyBird
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public bool IsGameOver { get; private set; }

        // 게임오버 이벤트(구독자들은 즉시 처리 가능)
        public event Action OnGameOver;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public void GameOver()
        {
            if (IsGameOver) return;
            IsGameOver = true;
            Debug.Log("Game Over");
            OnGameOver?.Invoke();
            // 필요한 후처리(사운드, UI 등) 여기에 추가
        }

        public void Restart()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}