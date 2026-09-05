using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    internal class PlayerNameInputModal : MonoBehaviour
    {
        [SerializeField] private float scaleInDuration = 0.4f;
        [SerializeField] private Ease scaleInCurve = Ease.EaseOutBounce;
        
        [SerializeField] private InputField _input;
        [SerializeField] private Button _submit;

        private CreditsManager _owner;

        #region Lifetime
        private void OnEnable()
        {
            _input.onValidateInput += ValidateNameInput;
        }

        private void Update()
        {
            _submit.interactable = _input.text.Length == 3;
        }

        private void OnDisable()
        {
            _input.onValidateInput -= ValidateNameInput;
        }
        #endregion
        
        public void Activate(CreditsManager owner)
        {
            _owner = owner;
            gameObject.SetActive(true);
            StartCoroutine(Tween.LerpLocalScale(transform, new Vector3(1f, 0, 1f), Vector3.one, scaleInDuration, scaleInCurve));
        }
        
        public void SubmitName()
        {
            _input.interactable = false;
            _submit.interactable = false;
            StartCoroutine(CloseThenDeactivate());
            _owner.SubmitScore(_input.text);
        }

        private IEnumerator CloseThenDeactivate()
        {
            yield return Tween.LerpLocalScale(transform, new Vector3(1f, 0, 1f), scaleInDuration, scaleInCurve);
            gameObject.SetActive(false);
        }

        private char ValidateNameInput(string str, int addedIndex, char addedChar)
        {
            if (!char.IsLetterOrDigit(addedChar))
            {
                return '\0';
            }

            if (str.Length > 2)
            {
                return '\0';
            }

            return addedChar;
        }
    }
}