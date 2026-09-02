using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Developer Profile")]
    public class SODeveloper : ScriptableObject
    {
        public string DeveloperName = "John Doe";
        public string role = "developer";
    }
}