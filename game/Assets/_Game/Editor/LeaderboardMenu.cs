using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    internal static class LeaderboardMenu
    {
        [MenuItem("Tools/Leaderboard/Get Top Scores")]
        private static void GetTopScores()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(LeaderboardClient.GetTopScores(
                response => Debug.Log($"GetTopScores succeeded: {JsonUtility.ToJson(response)}"),
                () => Debug.LogError("GetTopScores failed")));
        }

        [MenuItem("Tools/Leaderboard/Submit Test Score")]
        private static void SubmitTestScore()
        {
            const string initials = "TST";
            var score = Random.Range(0, 100000);
            EditorCoroutineUtility.StartCoroutineOwnerless(LeaderboardClient.SubmitScore(initials, score,
                response => Debug.Log($"SubmitScore succeeded: {JsonUtility.ToJson(response)}"),
                () => Debug.LogError("SubmitScore failed")));
        }
    }
}
