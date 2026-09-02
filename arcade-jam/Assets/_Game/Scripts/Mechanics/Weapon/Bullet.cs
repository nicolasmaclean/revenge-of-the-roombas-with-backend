using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Weapons
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        public bool CanHurtEnemies = false;

        [SerializeField]
        internal int damage = 1;

        [SerializeField]
        internal float speed = 2f;

        [SerializeField]
        internal int maxBounces = 3;

        [SerializeField]
        internal float lifespan = 20f;

        [Header("Feedback")]
        [SerializeField]
        internal UnityEvent OnActivate = null;

        [SerializeField]
        internal UnityEvent OnDeflect = null;

        [SerializeField]
        internal UnityEvent OnRefelct = null;

        System.Action deactivationCallback = null;
        float activationStamp = 0;
        int bounces = 0;

        Rigidbody _rigidbody = null;

        Vector3 velocity;
        float height;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            Physics.IgnoreLayerCollision(7, 8);
        }

        void OnCollisionEnter(Collision collision)
        {
            string otherLayer = LayerMask.LayerToName(collision.collider.gameObject.layer);

            switch (otherLayer)
            {
                case "Player":
                    Player.PlayerController.Instance.Damage(damage, transform.position);
                    Deactivate();
                    break;

                case "Enemy":
                    if (CanHurtEnemies)
                    {
                        Enemy.EnemyBase enemy = collision.collider.transform.parent.GetComponent<Enemy.EnemyBase>();
                        if (enemy == null) throw new System.NullReferenceException();

                        enemy.Kill();
                        break;
                    }
                    else
                    {
                        goto default;
                    }

                default:
                    if (bounces > maxBounces)
                    {
                        Deactivate();
                    }
                    else
                    {
                        bounces++;
                    }

                    transform.forward = collision.contacts[0].normal;
                    OnDeflect?.Invoke();
                    break;
            }
        }

        public void Activate(Vector3 initialPosition, Vector3 target, float speed = 0, int maxBounces = -1, float lifeSpan = -1, System.Action deactivationCallback = null)
        {
            height = initialPosition.y;
            this.enabled = true;
            activationStamp = Time.time;
            bounces = 0;

            transform.position = initialPosition;
            transform.forward = (target - initialPosition).normalized;

            if (speed != 0) this.speed = speed;
            if (maxBounces != -1) this.maxBounces = maxBounces;
            if (lifeSpan >= 0) this.lifespan = lifeSpan;
            if (deactivationCallback != null) this.deactivationCallback = deactivationCallback;

            OnActivate?.Invoke();
        }

        public void Reflect(Vector3 newDirection)
        {
            //_rigidbody.velocity = newDirection.normalized * speed;
            OnRefelct?.Invoke();
            Deactivate();
        }

        void Update()
        {
            if (Time.time - activationStamp > lifespan)
            {
                Deactivate();
            }
            else
            {
                Vector3 nPos = transform.position + transform.forward * speed * Time.deltaTime;
                nPos.y = height;
                transform.position = nPos;
            }
        }

        void Deactivate()
        {
            if (deactivationCallback == null)
            {
                Destroy(gameObject);
            }
            else
            {
                deactivationCallback();
            }
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
    }
}