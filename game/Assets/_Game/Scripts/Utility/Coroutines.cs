using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Utility
{
    public static class Coroutines
    {
        /// <summary>
        /// Wait <paramref name="seconds"/> before performing <paramref name="callback"/>
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator WaitBeforeCallback(float seconds, System.Action callback)
        {
            yield return new WaitForSeconds(seconds);

            callback();
            yield break;
        }

        public static IEnumerator WaitAFrameBeforeCallback(System.Action callback)
        {
            yield return null;
            callback();
            yield break;
        }

        public static IEnumerator WaitUntill(System.Func<bool> predicate, System.Action callback)
        {
            yield return new WaitUntil(predicate);
            callback();
            yield break;
        }

        public static IEnumerator LerpAlpha(CanvasGroup group, float from, float to, float duration, System.Action OnComplete = null)
        {
            // initial value
            group.alpha = from;

            // animate value
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                group.alpha = Mathf.Lerp(from, to, elapsedTime / duration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // final value
            group.alpha = to;
            if (OnComplete != null) { OnComplete(); }
            yield break;
        }

        public static IEnumerator LerpColor(MaskableGraphic graphic, Color from, Color to, float duration, System.Action OnComplete = null)
        {
            // initial value
            graphic.color = from;

            // animate value
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                graphic.color = Color.Lerp(from, to, elapsedTime / duration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // final value
            graphic.color = to;
            OnComplete?.Invoke();
            yield break;
        }

        public static IEnumerator ManageControlsUI(GameObject progressBar, GameObject button, GameObject levelManager, float fillSpeed, System.Action OnComplete = null)
        {
            yield return IncrementProgressBar(progressBar, fillSpeed);
            // disabling progress bar
            progressBar.SetActive(false);
            // activating button and level manager
            levelManager.SetActive(true);
            button.SetActive(true);
            yield return BlinkButton(button);

        }

        public static IEnumerator IncrementProgressBar(GameObject progressBar, float fillSpeed, System.Action OnComplete = null)
        {
            // getting slider
            Slider slider = progressBar.GetComponent<Slider>();
            // intial value
            slider.value = 0;

            while (slider.value < 1)
            {
                slider.value += fillSpeed * Time.deltaTime;
                yield return null;
            }

            // final value
            slider.value = 1;

            if (OnComplete != null) { OnComplete(); }
            yield break;
        }

        public static IEnumerator BlinkButton(GameObject button)
        {
            CanvasGroup group = button.GetComponent<CanvasGroup>();
            // initial value
            group.alpha = 1;

            bool clicked = false;
            // animate value
            while (!clicked)
            {
                group.alpha = 1;
                yield return new WaitForSeconds(0.6f);
                group.alpha = 0;
                yield return new WaitForSeconds(0.3f);
            }
        }

        public static IEnumerator LerpPosition(Transform target, Vector3 from, Vector3 to, float duration, System.Action OnComplete = null)
        {
            // initial value
            target.position = from;

            // animate value
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                target.position = Vector3.Lerp(from, to, elapsedTime / duration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // final value
            target.position = to;
            if (OnComplete != null) { OnComplete(); }
            yield break;
        }
    }
}