using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MissileCommand.Utils;
using System;

namespace MissileCommand
{
    public class RoundBreakMenu : Singleton<RoundBreakMenu>
    {
        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private TextMeshProUGUI _pointsInRoundText;
        [SerializeField] private TextMeshProUGUI _totalPointsText;
        [SerializeField] private TextMeshProUGUI _nextRoundText;

        private void OnEnable()
        {
            UpdateText();
        }

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
                _nextRoundText.text = $"Next round in: {RoundManager.Instance.BreakTimeLeft}s";
                yield return null;
            }
        }
    }
}
