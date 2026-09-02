using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Utility;

namespace Game
{
    public class ControlsUIManager : MonoBehaviour
    {
        #region private variables
        GameObject _canvas;
        GameObject _progressBar;
        GameObject _button;
        GameObject _levelManager;
        #endregion

        #region serialized variables
        [SerializeField] float _fillSpeed = 0.5f;
        #endregion

        void Start()
        {
            _canvas = GameObject.Find("Canvas");
            _progressBar = _canvas.transform.GetChild(0).gameObject;
            _button = _canvas.transform.GetChild(1).gameObject;
            _button.SetActive(false);
            _levelManager = _canvas.transform.GetChild(2).gameObject;
            _levelManager.SetActive(false);
            StartCoroutine(Coroutines.ManageControlsUI(_progressBar, _button, _levelManager, _fillSpeed));
        }
    }
}
