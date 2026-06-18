using System;
using UnityEngine;

namespace MyBird
{
    public class ScoreManager : MonoBehaviour
    {
        // 싱글톤
        public static ScoreManager Instance { get; private set; }

        // 현재 스코어
        public int Score { get; private set; }

        // 베스트 스코어 (PlayerPrefs로 저장 → 앱 껐다 켜도 유지)
        public int BestScore { get; private set; }

        // 이번 판에 신기록을 달성했는지
        public bool IsNewRecord { get; private set; }

        // 점수가 바뀔 때 발생하는 이벤트
        public event Action OnScoreChanged;

        // PlayerPrefs에서 베스트 스코어를 저장할 때 쓰는 키 이름
        const string BestScoreKey = "BestScore";

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // 저장된 베스트 스코어 불러오기 (없으면 0)
            BestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        }

        public void AddScore(int amount = 1)
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

            Score += amount;

            // 현재 스코어가 베스트 스코어를 넘으면 갱신
            if (Score > BestScore)
            {
                BestScore = Score;
                IsNewRecord = true;

                // PlayerPrefs에 저장 (즉시 디스크에 기록)
                PlayerPrefs.SetInt(BestScoreKey, BestScore);
                PlayerPrefs.Save();
            }

            OnScoreChanged?.Invoke();
        }
    }
}