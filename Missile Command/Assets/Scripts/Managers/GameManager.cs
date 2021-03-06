using UnityEngine;
using UnityEngine.SceneManagement;
using MissileCommand.Utils;

namespace MissileCommand.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        #region Events

        public delegate void GameStateEvent(GameState from, GameState to);
        public static event GameStateEvent OnGameStateChange;

        #endregion

        #region Variables
        public GameState CurrentGameState { get; private set; }

        private int currentSceneIndex = 0;
        private int previousSceneIndex = 0;

        private int sceneLoading = -1;
        private int sceneUnloading = -1;

        public enum GameState
        {
            Boot,
            MainMenu,
            Running,
            Paused,
            GameOver
        }

        #endregion

        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            CurrentGameState = GameState.Boot;
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            LoadLevel(1);
        }
        #endregion

        #region Private Methods

        private void ChangeGameState(GameState toState)
        {
            GameState previousState = CurrentGameState;
            CurrentGameState = toState;

            switch (toState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    break;
                case GameState.Running:
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0;
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0;
                    break;
            }

            OnGameStateChange?.Invoke(previousState, CurrentGameState);
        }

        private void LoadScene(int sceneIndex)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            ao.completed += OnLevelLoadComplete;
            sceneLoading = sceneIndex;
        }
        private void UnloadScene(int sceneIndex)
        {
            AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneIndex);
            ao.completed += OnLevelUnloadComplete;
            sceneUnloading = sceneIndex;
        }

        private void OnLevelUnloadComplete(AsyncOperation obj)
        {
            previousSceneIndex = sceneUnloading;
            sceneUnloading = -1;
        }

        private void OnLevelLoadComplete(AsyncOperation obj)
        {
            previousSceneIndex = currentSceneIndex;
            currentSceneIndex = sceneLoading;
            sceneLoading = -1;

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentSceneIndex));

            if (IsSceneUnloadable(previousSceneIndex))
                UnloadScene(previousSceneIndex);

            if (currentSceneIndex == 1)
                ChangeGameState(GameState.MainMenu);
            else if (currentSceneIndex == 2)
                ChangeGameState(GameState.Running);
        }
        private bool IsSceneLoadable(int sceneIndex) =>
            (sceneIndex > 0 && sceneIndex < SceneManager.sceneCountInBuildSettings) &&
            sceneIndex != currentSceneIndex;

        private bool IsSceneUnloadable(int sceneIndex) =>
            (sceneIndex < SceneManager.sceneCountInBuildSettings) &&
            sceneIndex != currentSceneIndex;

        #endregion

        #region Public Methods

        public void LoadLevel(int sceneIndex)
        {
            if (IsSceneLoadable(sceneIndex))
                LoadScene(sceneIndex);
        }

        public void StartGame() => LoadLevel(2);

        public void PauseToggle()
        {
            if (CurrentGameState == GameState.Running)
                ChangeGameState(GameState.Paused);

            else if (CurrentGameState == GameState.Paused)
                ChangeGameState(GameState.Running);
        }

        public void GameOver()
        {
            if (CurrentGameState == GameState.Running)
                ChangeGameState(GameState.GameOver);
        }

        #endregion
    }
}
