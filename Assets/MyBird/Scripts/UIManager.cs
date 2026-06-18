using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyBird
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI 오브젝트")]
        public GameObject readyUI;
        public GameObject gameOverUI;

        [Header("버튼")]
        public Button retryButton;
        public Button quitButton;

        [Header("게임오버 스코어 UI")]
        public TextMeshProUGUI currentScoreText;    // 현재 스코어 텍스트
        public TextMeshProUGUI bestScoreText;       // 베스트 스코어 텍스트
        public TextMeshProUGUI newRecordText;       // "NEW!" 텍스트 (평소엔 비활성화)

        [Header("NEW! 애니메이션 설정")]
        public float newAnimSpeed = 3f;             // 애니메이션 속도
        public float newAnimMinScale = 0.8f;        // 최소 크기
        public float newAnimMaxScale = 1.3f;        // 최대 크기

        Player player;
        bool gameOverUIShown = false;   // 게임오버 UI를 이미 띄웠는지 (중복 방지)
        float newAnimTime = 0f;         // 애니메이션 타이머

        void Awake()
        {
            player = FindObjectOfType<Player>();

            // 시작 시 상태 초기화
            if (readyUI != null) readyUI.SetActive(true);
            if (gameOverUI != null) gameOverUI.SetActive(false);
            if (newRecordText != null) newRecordText.gameObject.SetActive(false);

            // 리트라이 버튼
            if (retryButton != null)
                retryButton.onClick.AddListener(() => GameManager.Instance.Restart());

            // 퀴트 버튼 (메인씬이 생기면 SceneManager.LoadScene("Main") 등으로 교체)
            if (quitButton != null)
                quitButton.onClick.AddListener(() => Application.Quit());
        }

        void Update()
        {
            if (player == null) return;

            // Idle → Playing: ReadyUI 끄기
            if (player.State == Player.PlayerState.Playing)
            {
                if (readyUI != null && readyUI.activeSelf)
                    readyUI.SetActive(false);
            }

            // Dead 상태: GameOverUI 켜기
            if (player.State == Player.PlayerState.Dead)
            {
                if (!gameOverUIShown)
                {
                    ShowGameOverUI();
                    gameOverUIShown = true;
                }
            }

            // NEW! 애니메이션 업데이트
            if (newRecordText != null && newRecordText.gameObject.activeSelf)
            {
                AnimateNewRecord();
            }
        }

        void ShowGameOverUI()
        {
            if (gameOverUI != null)
                gameOverUI.SetActive(true);

            var sm = ScoreManager.Instance;
            if (sm == null) return;

            // 현재 스코어 표시
            if (currentScoreText != null)
                currentScoreText.text = "SCORE : " + sm.Score.ToString();

            // 베스트 스코어 표시
            if (bestScoreText != null)
                bestScoreText.text = "BESTSCORE : " + sm.BestScore.ToString();

            // 신기록이면 NEW! 텍스트 활성화
            if (sm.IsNewRecord && newRecordText != null)
            {
                newRecordText.gameObject.SetActive(true);
                newAnimTime = 0f;
            }
        }

        void AnimateNewRecord()
        {
            // Mathf.PingPong: 0 → 1 → 0 → 1 ... 으로 반복되는 값 생성
            newAnimTime += Time.deltaTime * newAnimSpeed;
            float t = Mathf.PingPong(newAnimTime, 1f);

            // t값으로 최소~최대 크기 사이를 왔다갔다
            float scale = Mathf.Lerp(newAnimMinScale, newAnimMaxScale, t);
            newRecordText.transform.localScale = Vector3.one * scale;
        }
    }
}