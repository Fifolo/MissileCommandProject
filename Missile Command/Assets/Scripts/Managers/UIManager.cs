using UnityEngine;
using TMPro;
using MissileCommand.Utils;

namespace MissileCommand
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Canvas _inGameCanvas;
        [SerializeField] private Canvas _roundBreakCanvas;
        [SerializeField] private Canvas _pauseMenuCanvas;
        [SerializeField] private Canvas _gameOverCanvas;

        private Canvas _currentCanvas;

        protected override void Awake()
        {
            base.Awake();

            _pauseMenuCanvas.gameObject.SetActive(false);
            _gameOverCanvas.gameObject.SetActive(false);
        }

        private void GameManager_OnGameStateChange(GameManager.GameState from, GameManager.GameState to)
        {
            if (to == GameManager.GameState.Paused)
                SwitchCanvas(_pauseMenuCanvas);

            else if (to == GameManager.GameState.Running)
                SwitchCanvas(_inGameCanvas);

            else if (to == GameManager.GameState.GameOver)
                SwitchCanvas(_gameOverCanvas);
        }

        protected void OnEnable() => SubscribeToEvents();
        protected void OnDisable() => UnsubscribeToEvents();

        private void SubscribeToEvents()
        {
            GameManager.OnGameStateChange += GameManager_OnGameStateChange;
            RoundManager.OnRoundStart += OnRoundStart;
            RoundManager.OnRoundFinish += OnRoundFinish;
        }


        private void UnsubscribeToEvents()
        {
            GameManager.OnGameStateChange -= GameManager_OnGameStateChange;
            RoundManager.OnRoundStart -= OnRoundStart;
            RoundManager.OnRoundFinish -= OnRoundFinish;
        }

        private void OnRoundFinish(int roundNumber) => SwitchCanvas(_roundBreakCanvas);

        private void OnRoundStart(int roundNumber)
        {
            _roundBreakCanvas.gameObject.SetActive(false);
            _inGameCanvas.gameObject.SetActive(true);
        }
        private void SwitchCanvas(Canvas newCanvas)
        {
            if (newCanvas)
            {
                if (_currentCanvas) _currentCanvas.gameObject.SetActive(false);

                _currentCanvas = newCanvas;
                _currentCanvas.gameObject.SetActive(true);
            }
        }
    }
}
