using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        #region public variables
        public int Health { get; private set; }
        public bool IsDead { get; private set; } = false;
        public Vector3 Velocity { get; set; } = Vector3.zero;

        public static PlayerController Instance = null;
        #endregion

        #region serialized variables
        [Header("Health")]
        [SerializeField]
        internal int maxHealth = 3;

        [SerializeField]
        internal UnityEvent OnHurt = null;

        [SerializeField]
        internal UnityEvent OnDeath = null;

        [Header("Movement")]
        [SerializeField]
        internal float moveSpeed = 10f;

        [SerializeField]
        internal string horizontalAxis = "Horizontal";

        [SerializeField]
        internal string verticalAxis = "Vertical";

        [SerializeField]
        internal UnityEvent OnMove = null;

        [Header("Dash")]
        [SerializeField]
        internal float dashForce = 75f;

        [SerializeField]
        [Tooltip("The number of seconds before dash may be performed again")]
        internal float dashCooldown = 2f;

        [SerializeField]
        internal string dashAxis = "Dash";

        [SerializeField]
        internal UnityEvent OnDash = null;

        [Header("Punch")]
        [SerializeField]
        internal Collider punchCollider = null;

        [SerializeField]
        internal string punchAxis = "Melee";

        [SerializeField]
        internal UnityEvent OnPunch = null;

        [Header("Counter")]
        [SerializeField]
        internal float counterRange = 2f;

        [SerializeField]
        internal string counterAxis = "Counter";

        [SerializeField]
        internal UnityEvent OnCounter = null;

        [Header("Camera")]
        [SerializeField]
        internal bool alignMovementToCamera = false;

        [SerializeField]
        internal Transform followCamera = null;

        [Header("Animations")]
        [SerializeField]
        internal Animator animator = null;
        #endregion

        #region private variables
        Rigidbody _rigidbody;

        Vector3 _inputMove = Vector3.zero;
        bool _dashIsCoolingdown = false;
        #endregion

        #region Monobehaviour
        void Awake()
        {
            if (alignMovementToCamera && followCamera == null)
            {
                Debug.LogWarning("alignMovementToCamera is true, but no followCamera was provided. " +
                    "alignMovementToCamera will false. Please provide a followCamera.");
                alignMovementToCamera = false;
            }

            if (punchCollider == null)
            {
                Debug.LogWarning("No punch collider was provided, so the punch attack will not anything");
            }
            else
            {
                punchCollider.enabled = false;
            }

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("There are multiple players in this scene, so " +
                    "PlayerMovement.Player may not work as intended.");
            }

            _rigidbody = GetComponent<Rigidbody>();
            Health = maxHealth;
        }

        void Update()
        {
            if (IsDead) return;
            // TODO: max speed?
            ApplyMovement();

            AttemptDash();
            AttemptPunch();
            //AttemptCounter();
        }

        void FixedUpdate()
        {
            if (IsDead) return;
            ApplyFixedMovement();
        }
        #endregion

        [SerializeField]
        Transform dashOrigin = null;

        [SerializeField]
        new Collider collider = null;

        #region public
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

        public void PlayVFXAtDashOrigin(UnityEngine.VFX.VisualEffectAsset vfxAsset)
        {
            if (dashOrigin == null) PlayVFX(vfxAsset);

            Transform t = VFX.OneShot.Play(vfxAsset).transform;
            t.position = dashOrigin.position;
            t.rotation = dashOrigin.rotation;
        }
        #endregion

        void ApplyMovement()
        {
            Vector2 movement = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));

            // apply perspective transformation
            if (alignMovementToCamera)
            {
                // transform y-axis movement
                Vector3 verticalDirection = Quaternion.AngleAxis(-90, Vector3.up) * followCamera.right;
                Vector2 flatForward = new Vector2(verticalDirection.x, verticalDirection.z);
                flatForward = flatForward.normalized;
                Vector2 nMovement = flatForward * movement.y;

                // transform x-axis movement
                flatForward = new Vector2(followCamera.right.x, followCamera.right.z);
                flatForward = flatForward.normalized;
                nMovement += flatForward * movement.x;

                movement = nMovement;
            }

            // cache movement
            _inputMove = new Vector3(movement.x, 0, movement.y);

            // apply rotation
            if (_inputMove == Vector3.zero) return;
            OnMove?.Invoke();
            _rigidbody.MoveRotation(Quaternion.LookRotation(_inputMove, Vector3.up));
        }

        void ApplyFixedMovement()
        {
            _rigidbody.MovePosition(transform.position + _inputMove * moveSpeed * Time.fixedDeltaTime);
        }

        /// <summary>
        /// Polls input and dashes forwards if appropriate
        /// </summary>
        /// <returns> true if a dash was performed, false otherwise </returns>
        bool AttemptDash()
        {
            if (!Input.GetButtonDown("Dash"))
            {
                return false;
            }

            // dash is still on cooldown
            if (_dashIsCoolingdown)
            {
                return false;
            }

            return Dash();
        }

        /// <summary>
        /// Applies force to <see cref="Velocity"/> in current movement direction.
        /// If the player is not moving, no dash will be performed.
        /// </summary>
        /// <returns> true if a dash was performed. </returns>
        bool Dash()
        {
            float characterVelocitySqr = _inputMove.sqrMagnitude;

            // player must be moving to dash in the same direction
            // so if the player is not moving, do not perform a dash.
            if (characterVelocitySqr == 0)
            {
                return false;
            }

            // dash animation
            if (animator != null)
            {
                if (!animator.GetCurrentAnimatorStateInfo(L_BASE).IsName(ST_IDLE))
                {
                    return false;
                }
                else
                {
                    animator.SetTrigger("Dashing");
                }
            }

            // set cooldown
            _dashIsCoolingdown = true;
            StartCoroutine(Utility.Coroutines.WaitBeforeCallback(dashCooldown, () =>
            {
                _dashIsCoolingdown = false;
            }));

            // apply dash force
            _rigidbody.AddRelativeForce(Vector3.forward * dashForce * 10f);
            OnDash?.Invoke();
            return true;
        }

        bool AttemptPunch()
        {
            if (!Input.GetButtonDown(punchAxis))
            {
                return false;
            }

            return Punch();
        }

        bool Punch()
        {
            if (animator != null)
            {
                if (!animator.GetCurrentAnimatorStateInfo(L_BASE).IsName(ST_IDLE))
                {
                    return false;
                }
                else
                {
                    animator.SetTrigger("Punching");
                    punchCollider.enabled = true;

                    StartCoroutine(Utility.Coroutines.WaitUntill(
                    () =>
                    {
                        return !animator.GetCurrentAnimatorStateInfo(L_BASE).IsName(ST_IDLE);
                    },
                    () =>
                    {
                        StartCoroutine(Utility.Coroutines.WaitUntill(
                        () =>
                        {
                            PunchCheck();
                            return animator.GetCurrentAnimatorStateInfo(L_BASE).IsName(ST_IDLE);
                        },
                        () =>
                        {
                            punchCollider.enabled = false;
                        }));
                    }));
                }
            }

            OnPunch?.Invoke();
            return true;
        }

        void PunchCheck()
        {
            if (Physics.CheckBox(collider.bounds.center, collider.bounds.extents / 2, Quaternion.identity, ~LayerMask.GetMask("Player")))
            {
                RaycastHit hitinfo;
                if (Physics.Raycast(collider.bounds.center, transform.forward, out hitinfo, 2, ~LayerMask.GetMask("Player")))
                {
                    HitCollider(hitinfo.collider);
                }
            }
        }

        void OnTriggerStay(Collider other) => HitCollider(other);

        void OnTriggerEnter(Collider other) => HitCollider(other);

        void OnTriggerExit(Collider other) => HitCollider(other);

        void HitCollider(Collider other)
        {
            if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
            {
                Enemy.EnemyBase enemy = other.transform.parent.GetComponent<Enemy.EnemyBase>();

                if (enemy != null)
                {
                    enemy.Kill();
                }
                else
                {
                    Debug.LogError("Unable to find enemy component to kill 'em");
                }
            }
        }

        bool AttemptCounter()
        {
            if (!Input.GetButtonDown(counterAxis))
            {
                return false;
            }

            return Counter();
        }

        bool Counter()
        {
            if (animator != null)
            {
                if (!animator.GetCurrentAnimatorStateInfo(L_BASE).IsName(ST_IDLE))
                {
                    return false;
                }
                else
                {
                    animator.SetTrigger("Countering");
                }
            }

            // reflect bullets
            foreach (Weapons.Bullet bullet in FindObjectsOfType<Weapons.Bullet>())
            {
                if (bullet != null && bullet.gameObject.activeInHierarchy)
                {
                    Vector3 fromPlayer = bullet.transform.position - transform.position;
                    if (fromPlayer.sqrMagnitude < counterRange * counterRange)
                    {
                        bullet.CanHurtEnemies = true;
                        bullet.Reflect(fromPlayer);
                    }
                }
            }

            OnCounter?.Invoke();
            return true;
        }

        public void Damage(int amt, Vector3 from)
        {
            if (IsDead) return;
            if (amt < 0) throw new System.ArgumentOutOfRangeException($"amt must be a non-negative integer. {amt} was provided.");
            Health -= amt;

            OnHurt.Invoke();

            if (Health <= 0)
            {
                Kill(from);
            }
        }

        public void Kill(Vector3 from)
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.useGravity = true;

            // apply death force
            _rigidbody.AddForce((transform.position - from).normalized * 500 + (Vector3.up * 200));
            _rigidbody.AddTorque(transform.right * 500);

            this.enabled = false;
            IsDead = true;

            OnDeath.Invoke();

            Health = 0;

            StartCoroutine(Utility.Coroutines.WaitBeforeCallback(4f,
            () =>
            {
                LevelManager.Instance.GoToNextLevel();
            }));
        }

        #region animator constants
        // animator layers
        const int L_BASE = 0;

        // animator states
        const string ST_IDLE = "Idle";

        // animator parameters
        const string T_DASH = "Dashing";
        const string T_COUNTER = "Countering";
        const string T_PUNCH = "Punching";
        #endregion
    }
}