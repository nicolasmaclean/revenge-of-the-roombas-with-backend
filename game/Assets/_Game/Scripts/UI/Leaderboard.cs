using System.Collections.Generic;
using System.Data;
using System.Linq;
using Game.Networking;
using UnityEngine;

namespace Game
{
    internal class Leaderboard : MonoBehaviour
    {
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _highlightColor = Color.yellow;
        
        [SerializeField] private LeaderboardRow _rowPrefab;

        private CreditsManager _owner;
        
        public void Config(CreditsManager owner, SubmitScoresResponse data, string initials, int score)
        {
            _owner = owner;
            gameObject.SetActive(true);
            Clear();
            Populate(data, initials, score);
        }

        public void GoToNextScreen()
        {
            gameObject.SetActive(false);
            _owner.ShowCredits();
        }

        private void Clear()
        {
            foreach (var child in GetComponentsInChildren<LeaderboardRow>())
            {
                Destroy(child.gameObject);
            }
        }

        private void Populate(SubmitScoresResponse data, string initials, int score)
        {
            for (var i = 0; i < data.scores.Count; i++)
            {
                ScoreEntry entry = data.scores[i];
                AddRow(i+1, entry.initials, entry.score, i+1 == data.rank ? _highlightColor : _normalColor);
            }

            // include the new we just added, if it's outside the top 10
            if (data.rank > data.scores.Count && data.rank > 0)
            {
                AddRow(data.rank, initials, score, _highlightColor);
            }
        }

        private void AddRow(int rank, string initials, int score, Color color)
        {
            var row = Instantiate(_rowPrefab, transform);
            row.Config(rank, initials, score, color);
        }
    }
}