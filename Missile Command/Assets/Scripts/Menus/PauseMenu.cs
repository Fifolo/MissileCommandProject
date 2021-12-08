using UnityEngine;
using UnityEngine.UI;
using MissileCommand.Utils;
using MissileCommand.Managers;

namespace MissileCommand.Menus
{
    public class PauseMenu : Singleton<PauseMenu>
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _menuButton;

        private void Start()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager)
            {
                _resumeButton.onClick.AddListener(gameManager.PauseToggle);
                _menuButton.onClick.AddListener(() => gameManager.LoadLevel(1));
            }
        }
    }
}
