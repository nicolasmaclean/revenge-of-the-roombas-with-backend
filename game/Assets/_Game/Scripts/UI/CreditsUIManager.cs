using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    public class CreditsUIManager : MonoBehaviour
    {
        [SerializeField]
        float _initialY = -490f;

        [SerializeField]
        float _finalY = 1100f;

        [SerializeField]
        float _crawlTime = 4f;

        [SerializeField]
        float _delayBetweenDevs = 1.25f;

        [SerializeField]
        float _startDelay = .5f;

        [SerializeField]
        float _endDelay = 1f;

        [SerializeField]
        List<SODeveloper> developers = new List<SODeveloper>();

        [SerializeField]
        DeveloperUI uiPrefab = null;

        [SerializeField]
        UnityEvent OnCreditsFinish = null;

        void Awake()
        {
            if (developers.Count == 0)
            {
                Debug.LogWarning($"Please provide a value for {nameof(developers)}");
                this.enabled = false;
            }
            if (uiPrefab == null)
            {
                Debug.LogWarning($"Please provide a value for {nameof(uiPrefab)}");
                this.enabled = false;
            }
        }

        void Start()
        {
            StartCoroutine(CreditsLoop());
        }

        IEnumerator CreditsLoop()
        {
            yield return new WaitForSeconds(_startDelay);

            List<SODeveloper> stackOfDevs = new List<SODeveloper>(developers);
            List<SODeveloper> randomDevs = new List<SODeveloper>();

            while (stackOfDevs.Count > 0)
            {
                int randomI = Random.Range(0, stackOfDevs.Count);
                randomDevs.Add(stackOfDevs[randomI]);
                stackOfDevs.RemoveAt(randomI);
            }

            foreach (SODeveloper dev in randomDevs)
            {
                if (dev == null) continue;

                // instantiate and show developer ui
                DeveloperUI ui = Instantiate(uiPrefab, transform);
                ui.ConfigureUI(dev);

                StartCoroutine(Crawl(ui));

                yield return new WaitForSeconds(_delayBetweenDevs);
            }

            while (_crawling > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(_endDelay);
            OnCreditsFinish.Invoke();
        }

        int _crawling = 0;

        IEnumerator Crawl(DeveloperUI ui)
        {
            _crawling++;

            // initial value
            RectTransform trans = ui.GetComponent<RectTransform>();
            Vector3 pos = trans.localPosition;

            pos.y = _initialY;
            trans.localPosition = pos;

            // animate value
            float elapsedTime = 0;
            while (elapsedTime < _crawlTime)
            {
                pos.y = Mathf.Lerp(_initialY, _finalY, elapsedTime / _crawlTime);
                trans.localPosition = pos;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // final value
            pos.y = _initialY;
            trans.localPosition = pos;

            _crawling--;
            Destroy(ui.gameObject);
            yield break;
        }
    }
}