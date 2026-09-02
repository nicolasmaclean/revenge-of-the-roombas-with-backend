using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility;

namespace Game.Enemy.States
{
    public class ShootState : State
    {
        EnemySniper _parent;
        Transform _target;
        float _preshotTime;
        float _postshotTime;
        Animator _animator;

        bool _shot = false;
        float _enterStamp = 0;
        bool _usingRandom = false;

        public ShootState(EnemySniper parent, Transform target, float preshotTime, float postshotTime, Animator animator = null)
        {
            _parent = parent;
            _target = target;
            _preshotTime  = preshotTime;
            _postshotTime = postshotTime;
            _animator = animator;
            if (_target == null) _usingRandom = true;
        }

        public void Enter()
        {
            _enterStamp = Time.time;
            _shot = false;
            _parent.agent.isStopped = true;

            if (_target == null)
            {
                // provide random target if not provided one
                _target = new GameObject("Random Target").transform;
                _target.gameObject.SetActive(false);
                _target.position = _parent.transform.position + Quaternion.Euler(0, Random.Range(-180, 180), 0) * _parent.transform.forward;
            }
        }

        public void Exit() { }

        public void Tick()
        {
            float elapsedTime = Time.time - _enterStamp;
            if (elapsedTime > _postshotTime + _preshotTime)
            {
                // exit
                if (_usingRandom)
                {
                    Object.Destroy(_target.gameObject);
                    _target = null;

                }
                _parent.TriggerTransition();
            }
            else if (elapsedTime > _preshotTime && !_shot)
            {
                // shoot
                if (_animator != null)
                {
                    _animator.SetTrigger("Shooting");
                }

                _parent.shootTarget = _usingRandom ? _parent.transform.position + _parent.transform.forward : _target.transform.position;
                if (_animator == null)
                {
                    _parent.Shoot();
                }

                _shot = true;
            }
            else if (elapsedTime < _preshotTime)
            {
                // turn _parent towards _target
                Vector3 newDirection = Vector3.RotateTowards(_parent.transform.forward, _target.position - _parent.transform.position, TURN_SPEED * Time.deltaTime, 0);
                _parent.transform.localRotation = Quaternion.LookRotation(newDirection);
            }
        }

        const float TURN_SPEED = 20f;
    }
}