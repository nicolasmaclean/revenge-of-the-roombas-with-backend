using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Game.VFX
{
    [RequireComponent(typeof(VisualEffect))]
    public class OneShot : MonoBehaviour
    {
        VisualEffect _vfx;

        public static OneShot Play(VisualEffectAsset vfxAsset)
        {
            GameObject go = new GameObject($"{vfxAsset.name} instance");
            VisualEffect vfx = go.AddComponent<VisualEffect>();
            vfx.visualEffectAsset = vfxAsset;

            return go.AddComponent<OneShot>();
        }

        void Awake()
        {
            _vfx = GetComponent<VisualEffect>();
            _vfx.Play();
            
            StartCoroutine(Utility.Coroutines.WaitBeforeCallback(.1f,
            () =>
            {
                StartCoroutine(Utility.Coroutines.WaitUntill(
                () =>
                {
                    return _vfx.aliveParticleCount == 0;
                },
                () =>
                {
                    Destroy(gameObject);
                }));
            }));
        }
    }
}