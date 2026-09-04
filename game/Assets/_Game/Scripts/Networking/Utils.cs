using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Networking
{
    internal static class Utils
    {
        public static string GetNonce()
        {
            return System.Guid.NewGuid().ToString("N");
        }

        public static UnityWebRequest PostJson(string url, string payload)
        {
            // UnityWebRequest.Post doesn't work when trying to send json. Only works with forms.
            var payloadBytes = Encoding.UTF8.GetBytes(payload);
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(payloadBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }
    }
}