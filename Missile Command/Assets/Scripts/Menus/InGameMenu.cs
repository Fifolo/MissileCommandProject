using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MissileCommand.Utils;

namespace MissileCommand
{
    public class InGameMenu : Singleton<InGameMenu>
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private void OnEnable()
        {
            ScoreManager.OnTotalScoreChange += OnTotalScoreChange;
        }

        private void OnDisable()
        {
            ScoreManager.OnTotalScoreChange -= OnTotalScoreChange;
        }
        private void OnTotalScoreChange(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}
