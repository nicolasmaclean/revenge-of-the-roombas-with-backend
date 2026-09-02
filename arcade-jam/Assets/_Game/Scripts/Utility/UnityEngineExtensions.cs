using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    public static class UnityEngineExtensions
    {
        public static List<Component> GetComponentsInAncestors(GameObject go, System.Type type)
        {
            List<Component> components = new List<Component>();
            Transform trans = go.transform.parent;

            while(trans != null)
            {
                Component component = trans.GetComponent(type);

                if (component != null)
                {
                    components.Add(component);
                }

                trans = trans.parent;
            }

            return components;
        }
    }
}