using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MissileCommand.Utils;

namespace MissileCommand
{
    public class GameOverMenu : Singleton<GameOverMenu>
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private Button _menuButton;

        private void Start()
        {
            if (GameManager.Instance)
                _menuButton.onClick.AddListener(() => GameManager.Instance.LoadLevel(1));
        }
        private void OnEnable()
        {
            _scoreText.text = $"Score: {ScoreManager.Instance.TotalScore}";
            _roundText.text = $"Rounds completed: {RoundManager.Instance.RoundNumber}";
        }
    }
}
