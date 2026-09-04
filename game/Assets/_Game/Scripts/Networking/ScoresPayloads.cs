using System.Collections.Generic;

namespace Game.Networking
{
    [System.Serializable]
    internal class ScoreEntry
    {
        public string initials;
        public int score;
        public string created_at;
    }

    [System.Serializable]
    internal class GetScoresResponse
    {
        public List<ScoreEntry> scores;
    }

    [System.Serializable]
    internal class SubmitScoresRequest
    {
        public string initials;
        public int score;
        public string nonce;
        public string signature;

        public SubmitScoresRequest(string initials, int score, string nonce, System.Func<string, string> hasher)
        {
            this.initials = initials;
            this.score = score;
            this.nonce = nonce;
            signature = hasher($"{initials}:{score}:{nonce}");
        }
    }
    
    [System.Serializable]
    internal class SubmitScoresResponse
    {
        public List<ScoreEntry> scores;
        public bool accepted;
        public int rank;
    }
}