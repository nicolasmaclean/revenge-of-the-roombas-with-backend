using Game.Networking;
using Game.UI;
using UnityEngine;

namespace Game
{
    internal class CreditsManager : MonoBehaviour
    {
        [SerializeField] private CreditsUIManager _creditsScroll;
        [SerializeField] private PlayerNameInputModal _nameInput;
        [SerializeField] private Leaderboard _leaderboard;
        [SerializeField] private LevelManager _levelManager;

        private void Start()
        {
            _creditsScroll.gameObject.SetActive(false);
            _leaderboard.gameObject.SetActive(false);
            _levelManager.enabled = false;
            _nameInput.Activate(this);
        }

        public void SubmitScore(string initials)
        {
            var score = ScoreManager.Instance?.Score ?? 0;
            StartCoroutine(LeaderboardClient.SubmitScore(
                initials, score, 
                response => _leaderboard.Config(this, response, initials, score),
                ErrorGettingLeaderboard
            ));
        }

        public void ShowCredits()
        {
            _creditsScroll.gameObject.SetActive(true);
            _levelManager.enabled = true;
        }

        private void ErrorGettingLeaderboard()
        {
            Debug.LogError("Error while submitting score...");
            ShowCredits();
        }
    }
}
