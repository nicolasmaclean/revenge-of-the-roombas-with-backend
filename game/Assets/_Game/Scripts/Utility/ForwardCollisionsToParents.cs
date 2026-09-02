using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    [RequireComponent(typeof(Collider))]
    public class ForwardCollisionsToParents : MonoBehaviour
    {
        float timestamp = 0;

        void Awake()
        {
            List<Component> ancestors = UnityEngineExtensions.GetComponentsInAncestors(gameObject, typeof(Collider));
            Collider thisCollider = GetComponent<Collider>();

            foreach (Collider ancestor in ancestors)
            {
                Physics.IgnoreCollision(thisCollider, ancestor, true);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (timestamp == Time.time) return;

            timestamp = Time.time;
            SendMessageUpwards(nameof(OnCollisionEnter), collision);
        }

        void OnCollisionStay(Collision collision)
        {
            if (timestamp == Time.time) return;

            timestamp = Time.time;
            SendMessageUpwards(nameof(OnCollisionStay), collision);
        }

        void OnCollisionExit(Collision collision)
        {
            if (timestamp == Time.time) return;

            timestamp = Time.time;
            SendMessageUpwards(nameof(OnCollisionExit), collision);
        }


        void OnTriggerEnter(Collider other)
        {
            if (timestamp == Time.time) return;

            timestamp = Time.time;
            SendMessageUpwards(nameof(OnTriggerEnter), other);
        }

        void OnTriggerStay(Collider other)
        {
            if (timestamp == Time.time) return;

            timestamp = Time.time;
            SendMessageUpwards(nameof(OnTriggerStay), other);
        }

        void OnTriggerExit(Collider other)
        {
            if (timestamp == Time.time) return;

            timestamp = Time.time;
            SendMessageUpwards(nameof(OnTriggerExit), other);
        }
    }
}