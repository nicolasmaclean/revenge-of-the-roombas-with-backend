using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility;

namespace Game.Enemy.States
{
    public class MoveState : State
    {
        EnemyBase _parent;
        Transform _target;
        float _range;

        /// <summary>
        /// Moves <paramref name="parent"/> within <paramref name="range"/> of <paramref name="target"/>.
        /// If <paramref name="target"/> is null, <paramref name="parent"/> will move randomly <paramref name="range"/> units.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="target"></param>
        /// <param name="range"></param>
        public MoveState(EnemyBase parent, Transform target, float range)
        {
            _parent = parent;
            _target = target;
            _range = range;
        }

        public void Enter()
        {
            _parent.agent.isStopped = false;

            if (_target == null)
            {
                // set random destination
                _parent.agent.stoppingDistance = 0;
                _parent.agent.SetDestination(Quaternion.Euler(0, Random.Range(-180, 180), 0) * _parent.transform.forward * _range);
            }
            else
            {
                // back away from target
                Vector3 direction = _parent.transform.position - _target.transform.position;
                float distance = direction.magnitude;
                if (distance < _range)
                {
                    _parent.agent.stoppingDistance = 0;
                    _parent.agent.SetDestination(_target.position + direction * (_range - distance / 2));
                }
                // move towards target
                else
                {
                    _parent.agent.stoppingDistance = _range;
                    _parent.agent.SetDestination(_target.position);
                }
            }
        }

        public void Exit() { }

        public void Tick()
        {
            if (_parent.agent.remainingDistance < _range + RANGE_BUFFER)
            {
                _parent.TriggerTransition();
            }
        }

        const float RANGE_BUFFER = .1f;
    }
}