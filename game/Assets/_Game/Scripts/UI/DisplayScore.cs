using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(Text))]
    public class DisplayScore : MonoBehaviour
    {
        Text _textBox;

        void Start()
        {
            _textBox = GetComponent<Text>();
            if (ScoreManager.Instance != null)
            {
                _textBox.text = ScoreManager.Instance.Score.ToString();
            }
        }
    }
}
