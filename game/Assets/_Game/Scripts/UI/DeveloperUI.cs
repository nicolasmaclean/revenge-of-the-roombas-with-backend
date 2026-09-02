using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DeveloperUI : MonoBehaviour
    {
        [SerializeField]
        SODeveloper _developerData = null;

        [Header("UI References")]
        [SerializeField]
        Text _txt_name = null;

        [SerializeField]
        Text _txt_role = null;

        public void ConfigureUI(SODeveloper developerData)
        {
            _developerData = developerData;
            ConfigureUI();
        }

        public void ConfigureUI()
        {
            if (_developerData == null)
            {
                Debug.LogWarning($"Please provide a value for {nameof(_developerData)}");
                return;
            }

            _txt_name.text = _developerData.DeveloperName;
            _txt_role.text = _developerData.role;
        }
    }
}