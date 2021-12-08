using UnityEngine;
using TMPro;
using MissileCommand.Utils;
using MissileCommand.Managers;

namespace MissileCommand.Menus
{
    public class InGameMenu : Singleton<InGameMenu>
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _ammoText;

        #endregion

        #region MonoBehaviour

        private void OnEnable() => SubscribeToEvents();
        private void OnDisable() => UnSubscribeToEvents();

        #endregion

        #region Private Methods

        private void SubscribeToEvents()
        {
            PlayerController.OnPlayerFire += OnPlayerFire;
            ScoreManager.OnTotalScoreChange += OnTotalScoreChange;
            RoundManager.OnRoundStart += OnRoundStart;
        }

        private void UnSubscribeToEvents()
        {
            PlayerController.OnPlayerFire -= OnPlayerFire;
            ScoreManager.OnTotalScoreChange -= OnTotalScoreChange;
            PlayerController.OnPlayerFire -= OnPlayerFire;
        }

        private void OnRoundStart(int roundNumber) => UpdateAmmoText(MissilesManager.Instance.PlayerMissilesPerRound);

        private void OnPlayerFire(int ammoLeft) => UpdateAmmoText(ammoLeft);

        private void UpdateAmmoText(int ammoLeft) => _ammoText.text = $"Ammo left: {ammoLeft}";

        private void OnTotalScoreChange(int score) => _scoreText.text = score.ToString();

        #endregion
    }
}
