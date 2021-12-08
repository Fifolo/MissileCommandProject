using UnityEngine;
using MissileCommand.Utils;

namespace MissileCommand.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        #region Variables

        [SerializeField] private Canvas _inGameCanvas;
        [SerializeField] private Canvas _roundBreakCanvas;
        [SerializeField] private Canvas _pauseMenuCanvas;
        [SerializeField] private Canvas _gameOverCanvas;

        private Canvas _currentCanvas;

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();

            _pauseMenuCanvas.gameObject.SetActive(false);
            _gameOverCanvas.gameObject.SetActive(false);
        }

        #endregion

        #region Private Methods

        private void GameManager_OnGameStateChange(GameManager.GameState from, GameManager.GameState to)
        {
            if (to == GameManager.GameState.Paused)
                SwitchCanvas(_pauseMenuCanvas);

            else if (to == GameManager.GameState.Running)
                SwitchCanvas(_inGameCanvas);

            else if (to == GameManager.GameState.GameOver)
                SwitchCanvas(_gameOverCanvas);
        }

        private void OnEnable() => SubscribeToEvents();
        private void OnDisable() => UnsubscribeToEvents();

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
            SwitchCanvas(_inGameCanvas);
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

        #endregion
    }
}
