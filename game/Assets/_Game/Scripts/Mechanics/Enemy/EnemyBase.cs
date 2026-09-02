using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Game.Enemy
{
    [RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
    public abstract class EnemyBase : StateMachineBase
    {
        public static int EnemyCount = 0;
        public bool Dead { get; private set; } = false;

        [SerializeField]
        protected int score = 100;

        [SerializeField]
        internal UnityEvent OnSpawn = null;

        [SerializeField]
        internal UnityEvent OnDeath = null;

        [SerializeField]
        internal new Collider collider = null;

        [SerializeField]
        protected new Renderer renderer = null;

        const float tossTime = 2.5f;

        internal NavMeshAgent agent;
        protected new Rigidbody rigidbody;
        protected Material[] materials;

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();

            materials = renderer.materials;

            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            OnAwake();
            Spawn();
        }

        protected virtual void OnAwake() { }

        public abstract void TriggerTransition();

        public virtual void Kill()
        {
            if (Dead) return;
            Dead = true;
            StopAllCoroutines();

            OnDeath?.Invoke();

            rigidbody.constraints = RigidbodyConstraints.None;

            Vector3 fromPlayer = transform.position - Player.PlayerController.Instance.transform.position;
            fromPlayer = fromPlayer.normalized * 500f;

            rigidbody.AddRelativeForce(fromPlayer);

            StartCoroutine(Coroutines.WaitBeforeCallback(tossTime,
            () =>
            {
                collider.enabled = false;
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                StartCoroutine(Coroutines.LerpPosition(transform, transform.position, transform.position - Vector3.up * 2f, 1.5f,
                () =>
                {
                    Destroy(gameObject);
                }));
            }));

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.Score += score;
            }

            foreach (Material mat in materials)
            {
                mat.color = mat.color * .3f;
            }

            this.enabled = false;
            EnemyCount--;
        }

        public virtual void Spawn()
        {
            OnSpawn?.Invoke();
            EnemyCount++;
        }

        public void PlayClip(Audio.AudioClipData audioclip)
        {
            if (audioclip == null) throw new System.ArgumentNullException();
            audioclip.Play();
        }

        public void PlayVFX(UnityEngine.VFX.VisualEffectAsset vfxAsset)
        {
            if (vfxAsset == null) throw new System.ArgumentNullException();

            Transform t = VFX.OneShot.Play(vfxAsset).transform;
            t.position = transform.position + Vector3.up;
        }

        protected IEnumerator LerpMaterialsTo(Color finalColor, float duration, System.Action OnComplete = null)
        {
            Color[] initColors = new Color[materials.Length];
            for (int i = 0; i < initColors.Length; i++)
            {
                initColors[i] = materials[i].color;
            }

            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].color = Color.Lerp(initColors[i], finalColor, elapsedTime / duration);
                }

                yield return null;
                elapsedTime += Time.deltaTime;
            }

            foreach (Material mat in materials)
            {
                mat.color = finalColor;
            }

            OnComplete?.Invoke();
            yield break;
        }
    }
}