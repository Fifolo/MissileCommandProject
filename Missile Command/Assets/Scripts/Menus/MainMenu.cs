using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MissileCommand.Utils;
using UnityEngine.UI;


namespace MissileCommand
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
