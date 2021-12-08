using UnityEngine;
using MissileCommand.Utils;
using UnityEngine.UI;
using MissileCommand.Managers;

namespace MissileCommand.Menus
{
    public class MainMenu : Singleton<MainMenu>
    {
        [SerializeField] private Button _playButton;

        private void Start()
        {
            if (GameManager.Instance)
                _playButton.onClick.AddListener(GameManager.Instance.StartGame);
        }
    }
}
