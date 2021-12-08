using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MissileCommand.Utils;
using MissileCommand.Managers;

namespace MissileCommand.Menus
{
    public class GameOverMenu : Singleton<GameOverMenu>
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private Button _menuButton;
        #endregion

        #region MonoBehaviour

        private void Start()
        {
            if (GameManager.Instance)
                _menuButton.onClick.AddListener(() => GameManager.Instance.LoadLevel(1));
        }
        private void OnEnable()
        {
            _scoreText.text = $"Score: {ScoreManager.Instance.TotalScore}";
            _roundText.text = $"Final Round: {RoundManager.Instance.RoundNumber}";
        }

        #endregion
    }
}
