using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Game.Utility;
using Game.Player;

namespace Game.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyBomb : EnemyBase
    {
        #region states
        protected State st_wait;
        protected State st_seek;
        protected State st_fuse;
        #endregion

        #region serialized variables
        [Header("Controls")]
        [SerializeField] float _waitTime = 1f;
        [SerializeField] float _fuseTime = 2f;
        [SerializeField] float _fuseRange = 1.5f;
        [SerializeField] float _explosionRange = 3f;

        [Header("Feedback")]
        [SerializeField] UnityEvent OnStartFuse = null;
        [SerializeField] UnityEvent OnBlowUp    = null;
        #endregion

        void Start()
        {
            st_wait = new States.WaitState(this, _waitTime);
            st_seek = new States.SeekState(this, PlayerController.Instance.transform, _fuseRange);
            st_fuse = new States.WaitState(this, _fuseTime);

            ChangeState(st_wait);
        }

        public override void TriggerTransition()
        {
            if (CurrentState == st_wait)
            {
                ChangeState(st_seek);
            }
            else if (CurrentState == st_seek)
            {
                OnStartFuse?.Invoke();
                StartCoroutine(LerpMaterialsTo(Color.red, _fuseTime));
                ChangeState(st_fuse);
            }
            else
            {
                if (Dead) return;
                BlowUp();
            }
        }

        void BlowUp()
        {
            if (Dead) return;
            // friendly fire
            foreach (EnemyBase enemy in FindObjectsOfType<EnemyBase>())
            {
                if (enemy == this) continue;
                float distanceSQ = (transform.position - enemy.transform.position).sqrMagnitude;
                if (distanceSQ < _explosionRange * _explosionRange)
                {
                    enemy.Kill();
                }
            }

            // hurt player
            float distanceSq = (PlayerController.Instance.transform.position - transform.position).sqrMagnitude;
            if (distanceSq < _explosionRange * _explosionRange)
            {
                PlayerController.Instance.Damage(5, transform.position);
            }

            OnBlowUp?.Invoke();
            Destroy(gameObject);
        }

        public override void Kill()
        {
            if (Dead) return;

            agent.enabled = false;
            base.Kill();
        }
    }
}
