using System.Collections;
using System.Security.Cryptography;
using System.Text;
using Game.Networking;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    internal class LeaderboardClient
    {
        private const string URL = "http://localhost:8080";
        
        // WARNING
        // In practice, this should never ever be in a public repo.
        // It's trivial for user to decompile this from distributed game and this is only used against a locally hosted
        // backend instance so it's not a big deal for this use case.
        private const string SECRET = "c91cacabb4cc0b483efebd4a596d03a284ebaabca827599bf47b2c9fc286787c"; 
        
        internal static IEnumerator GetTopScores(System.Action<GetScoresResponse> onSuccess, [CanBeNull] System.Action onError = null)
        {
            using var request = UnityWebRequest.Get($"{URL}/scores");
            request.timeout = 5;
            
            // send request and wait!
            yield return request.SendWebRequest();
            
            // handle web request errors
            switch (request.result)
            {
                case UnityWebRequest.Result.InProgress:
                    throw new System.SystemException("This should never happen, the above yield runs till complete/error.");
                
                case UnityWebRequest.Result.Success:
                    // no-op, we'll continue process below
                    break;
                
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError: // 4xx or 5xx response
                case UnityWebRequest.Result.DataProcessingError:
                default:
                    onError?.Invoke();
                    yield break;
            }
            
            // request was successful
            try
            {
                var response = JsonUtility.FromJson<GetScoresResponse>(request.downloadHandler.text);
                onSuccess(response);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
        }
        
        internal static IEnumerator SubmitScore(string initials, int score, System.Action<SubmitScoresResponse> onSuccess, [CanBeNull] System.Action onError = null)
        {
            var payload = new SubmitScoresRequest(initials, score, Utils.GetNonce(), GetHash);
            using var request = Utils.PostJson($"{URL}/scores", JsonUtility.ToJson(payload));
            request.timeout = 5;
            
            // send request and wait!
            yield return request.SendWebRequest();
            
            // handle web request errors
            switch (request.result)
            {
                case UnityWebRequest.Result.InProgress:
                    throw new System.SystemException("This should never happen, the above yield runs till complete/error.");
                
                case UnityWebRequest.Result.Success:
                    // no-op, we'll continue process below
                    break;
                
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError: // 4xx or 5xx response
                case UnityWebRequest.Result.DataProcessingError:
                default:
                    onError?.Invoke();
                    yield break;
            }
            
            // request was successful
            try
            {
                var response = JsonUtility.FromJson<SubmitScoresResponse>(request.downloadHandler.text);
                onSuccess(response);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
        }

        private static string GetHash(string value)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SECRET));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            
            // manually compute hex encoding, this version of c# doesn't have Convert.ToHexString() T_T
            var sb = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}