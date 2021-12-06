using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MissileCommand.Utils;

namespace MissileCommand
{
    public class RoundBreakMenu : Singleton<RoundBreakMenu>
    {
        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private TextMeshProUGUI _pointsInRoundText;
        [SerializeField] private TextMeshProUGUI _totalPointsText;

        private void OnEnable()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            _roundText.text = $"Round {RoundManager.Instance.RoundNumber}";
            _pointsInRoundText.text = $"Points this round: {ScoreManager.Instance.ScoreThisRound}";
            _totalPointsText.text = $"Total Points: {ScoreManager.Instance.TotalScore}";
        }
    }
}
