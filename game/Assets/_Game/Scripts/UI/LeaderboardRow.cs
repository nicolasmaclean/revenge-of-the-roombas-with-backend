using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    internal class LeaderboardRow : MonoBehaviour
    {
        [SerializeField] private Text _rank;
        [SerializeField] private Text _name;
        [SerializeField] private Text _score;

        public void Config(int rank, string initials, int score, Color color)
        {
            _rank.text = rank.ToString();
            _rank.color = color;
            _name.text = initials;
            _name.color = color;
            _score.text = score.ToString();
            _score.color = color;
        }
    }
}