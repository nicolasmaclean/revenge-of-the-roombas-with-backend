using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Utility;

namespace Game
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;
        int _score;
        const float MOVE_UP_DISTANCE = 100f;

        [SerializeField]
        internal GameObject _textPrefab;

        [SerializeField]
        internal Canvas _canvas;

        [SerializeField]
        internal float _fadeTime = 1f;

        [SerializeField]
        internal float _scrollTime = 1.5f;

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                int oldScore = _score;
                _score = value;
                DisplayScoreUpdate(oldScore);
            }
        }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
                Instance = null;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void DisplayScoreUpdate(int oldScore)
        {
            GameObject textObj = Instantiate(_textPrefab, _canvas.transform);
            // Setting text
            Text textComp = textObj.GetComponent<Text>();
            textComp.text = "+" + (_score - oldScore).ToString();
            // Getting canvas group
            CanvasGroup group = textObj.GetComponent<CanvasGroup>();
            // Getting end position of text upward movement
            Vector3 textStartPosition = textObj.transform.position;
            Vector3 textEndPosition = new Vector3(textStartPosition.x, textStartPosition.y + MOVE_UP_DISTANCE, textStartPosition.z);

            // Fading and moving text upwards
            StartCoroutine(Coroutines.LerpAlpha(group, 1, 0, _fadeTime));
            StartCoroutine(Coroutines.LerpPosition(textObj.transform, textStartPosition, textEndPosition, _scrollTime,
                () =>
                {
                    Destroy(textObj);
                }));
        }
    }
}