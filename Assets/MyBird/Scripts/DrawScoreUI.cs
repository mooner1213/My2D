using UnityEngine;
using TMPro;

namespace MyBird
{
    public class DrawScoreUI : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;   // Inspector에서 텍스트 오브젝트 연결

        void Update()
        {
            if (scoreText == null || ScoreManager.Instance == null) return;

            // 매 프레임 ScoreManager에서 점수를 읽어서 텍스트에 반영
            scoreText.text = ScoreManager.Instance.Score.ToString();
        }
    }
}