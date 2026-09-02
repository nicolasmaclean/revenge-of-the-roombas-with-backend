using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.States;
using Game.Weapons;
using UnityEngine.Events;

namespace Game.Enemy
{
    public class EnemySniper : EnemyBase
    {
        static BulletPool BulletPool = null;

        [Header("AI")]
        [SerializeField]
        internal float superRange = 15f;

        [SerializeField]
        internal float firingRange = 5f;

        [SerializeField]
        internal int randomShots = 3;

        [SerializeField]
        internal float shotInDuration = .3f;

        [SerializeField]
        internal float shotOutDuration = .1f;

        [Header("Bullets")]
        [SerializeField]
        internal float bulletSpeed = 4f;

        [SerializeField]
        internal int maxBounces = 3;

        [SerializeField]
        internal Transform bulletOrigin = null;

        [SerializeField]
        internal Bullet bulletPrefab = null;

        [Header("Feedback")]
        [SerializeField]
        internal UnityEvent OnRandomShot = null;

        [SerializeField]
        internal UnityEvent OnTargetedShot = null;

        [SerializeField]
        internal Animator animator = null;

        [SerializeField]
        internal UnityEngine.VFX.VisualEffectAsset vfxAsset = null;

        MoveState _moveIntoSuperRange;
        MoveState _moveIntoRange;
        ShootState _shootRandomState_1;
        ShootState _shootRandomState_2;
        ShootState _shootTargetState;

        int shotsDone = 0;

        protected override void Awake()
        {
            base.Awake();
            if (BulletPool == null && bulletPrefab != null)
            {
                BulletPool = BulletPool.CreatePool(bulletPrefab);
            }

            if (bulletOrigin == null)
            {
                Debug.LogWarning($"{this}: no bulletOrigin was provided, so firing will be disabled.");
            }
        }

        void Start()
        {
            _moveIntoSuperRange = new MoveState(       this, Player.PlayerController.Instance.transform, superRange);
            _moveIntoRange      = new MoveState(       this, Player.PlayerController.Instance.transform, firingRange);
            _shootRandomState_1 = new ShootState(this, null, shotInDuration, shotOutDuration, animator);
            _shootRandomState_2 = new ShootState(this, null, shotInDuration, shotOutDuration, animator);
            _shootTargetState   = new ShootState(this, Player.PlayerController.Instance.transform, shotInDuration, shotOutDuration);

            ChangeState(_moveIntoSuperRange);
        }

        public override void TriggerTransition()
        {
            // move into super-range -> random shot
            if (CurrentState == _moveIntoSuperRange)
            {
                ChangeState(_shootRandomState_1);
                shotsDone++;
            }
            else if (CurrentState == _shootRandomState_2 || CurrentState == _shootRandomState_1 || CurrentState == _shootTargetState)
            {
                ChangeState(_moveIntoRange);
            }
            else if (shotsDone < randomShots)
            {
                // alternate between them to ensure it is not skipped as a duplicate
                ChangeState(shotsDone % 2 == 1 ? _shootRandomState_2 : _shootRandomState_1);
                shotsDone++;
                OnRandomShot?.Invoke();
            }
            else
            {
                ChangeState(_shootTargetState);
                shotsDone = 0;
                OnTargetedShot?.Invoke();
            }
        }

        internal Vector3 shootTarget = Vector3.zero;
        public void Shoot()
        {
            if (bulletOrigin == null) return;

            Bullet bullet = BulletPool.ActivateFromPool();
            bullet.gameObject.SetActive(true);

            Vector3 offset = Vector3.up * VERTICAL_OFFSET;

            bullet.Activate(bulletOrigin.position, shootTarget + offset, bulletSpeed, maxBounces, deactivationCallback:
            () =>
            {
                bullet.gameObject.SetActive(false);
                BulletPool.ReturnToPool(bullet);
            });

            if (vfxAsset == null) return;

            Transform t = VFX.OneShot.Play(vfxAsset).transform;
            t.position = bulletOrigin.transform.position + Vector3.up;
            t.rotation = bulletOrigin.transform.rotation;
        }

        public const float VERTICAL_OFFSET = .5f;
    }
}