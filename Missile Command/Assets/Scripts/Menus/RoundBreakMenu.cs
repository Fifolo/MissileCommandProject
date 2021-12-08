using System.Collections;
using UnityEngine;
using TMPro;
using MissileCommand.Utils;
using MissileCommand.Managers;

namespace MissileCommand.Menus
{
    public class RoundBreakMenu : Singleton<RoundBreakMenu>
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private TextMeshProUGUI _pointsInRoundText;
        [SerializeField] private TextMeshProUGUI _totalPointsText;
        [SerializeField] private TextMeshProUGUI _nextRoundText;

        #endregion

        #region MonoBehaviour
        private void OnEnable() => UpdateText();
        #endregion

        #region Private Methods
        private void UpdateText()
        {
            _roundText.text = $"Round {RoundManager.Instance.RoundNumber}";
            _pointsInRoundText.text = $"Points this round: {ScoreManager.Instance.ScoreThisRound}";
            _totalPointsText.text = $"Total Points: {ScoreManager.Instance.TotalScore}";

            if (RoundManager.Instance)
                StartCoroutine(UpdateTimer());

            else _nextRoundText.gameObject.SetActive(false);
        }

        private IEnumerator UpdateTimer()
        {
            while (gameObject.activeInHierarchy)
            {
                _nextRoundText.text = $"Next round in: {RoundManager.Instance.BreakTimeLeft.ToString("0.00")}s";
                yield return null;
            }
        }
        #endregion
    }
}
