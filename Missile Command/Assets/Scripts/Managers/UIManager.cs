using UnityEngine;
using TMPro;
using MissileCommand.Utils;

namespace MissileCommand
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("In Game Properties")]
        [SerializeField] private Canvas _inGameCanvas;
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Header("Round Break Properties")]
        [SerializeField] private Canvas _roundBreakCanvas;
        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private TextMeshProUGUI _pointsInRoundText;
        [SerializeField] private TextMeshProUGUI _totalPointsText;


        protected override void Awake()
        {
            base.Awake();
            ScoreManager.OnTotalScoreChange += OnTotalScoreChange;
            RoundManager.OnRoundFinish += OnRoundFinish;
        }

        private void OnRoundFinish(int roundNumber)
        {
            _inGameCanvas.gameObject.SetActive(false);
            _roundBreakCanvas.gameObject.SetActive(true);

            _roundText.text = $"Round {roundNumber}";
            _pointsInRoundText.text = $"Points this round: {ScoreManager.Instance.ScoreThisRound}";
            _totalPointsText.text = $"Total Points: {ScoreManager.Instance.TotalScore}";
        }

        private void OnTotalScoreChange(int score)
        {
            _scoreText.text = ScoreManager.Instance.TotalScore.ToString();
        }

        protected void OnEnable()
        {
            RoundManager.OnRoundStart += OnRoundStart;
            RoundManager.OnRoundFinish += OnRoundFinish;
        }
        protected void OnDisable()
        {
            RoundManager.OnRoundStart -= OnRoundStart;
            RoundManager.OnRoundFinish -= OnRoundFinish;
        }
        private void OnRoundStart(int roundNumber)
        {
            _roundBreakCanvas.gameObject.SetActive(false);
            _inGameCanvas.gameObject.SetActive(true);
        }
    }
}
