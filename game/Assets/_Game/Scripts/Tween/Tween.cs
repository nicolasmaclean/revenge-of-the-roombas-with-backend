using System.Collections;
using UnityEngine;

namespace Game
{
    internal static class Tween
    {
        public static IEnumerator LerpLocalScale(Transform transform, Vector3 start, Vector3 target, float duration, Ease curve)
        {
            transform.localScale = start;
            float elapsed = 0;
            while (elapsed < duration)
            {
                yield return null;
                elapsed += Time.deltaTime;

                float t = elapsed / duration;
                transform.localScale = Vector3.Lerp(start, target, curve.Eval(t));
            }

            transform.localScale = target;
        }
        
        public static IEnumerator LerpLocalScale(Transform transform, Vector3 target, float duration, Ease curve)
        {
            Vector3 start = transform.localScale;
            float elapsed = 0;
            while (elapsed < duration)
            {
                yield return null;
                elapsed += Time.deltaTime;

                float t = elapsed / duration;
                transform.localScale = Vector3.Lerp(start, target, curve.Eval(t));
            }

            transform.localScale = target;
        }
    }
}