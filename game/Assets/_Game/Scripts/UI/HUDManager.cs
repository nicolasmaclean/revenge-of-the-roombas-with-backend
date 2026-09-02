using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField]
        Player.PlayerController player;

        [SerializeField]
        Text txt_score;

        [SerializeField]
        MaskableGraphic graphic = null;

        [SerializeField]
        List<GameObject> hearts = new List<GameObject>();

        void Start()
        {
            UpdateHUD();
        }

        void Update()
        {
            UpdateHUD();
        }

        void UpdateHUD()
        {
            txt_score.text = ScoreManager.Instance.Score.ToString();
            for (int i = 0; i < hearts.Count; i++)
            {
                if (i >= player.Health)
                {
                    hearts[i].SetActive(false);
                }
                else
                {
                    hearts[i].SetActive(true);
                }
            }
        }

        public void Hurt()
        {
            if (graphic == null) return;
            StopAllCoroutines();
            StartCoroutine(Utility.Coroutines.LerpColor(graphic, new Color(1, 0, 0, .7f), new Color(1, 0, 0, 0), .5f));
        }

        public void Killed()
        {
            if (graphic == null) return;
            StopAllCoroutines();
            graphic.color = new Color(1, 0, 0, .2f);
        }
    }
}