using Game.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy.States
{
    public class SeekState : State
    {
        #region instance variables
        EnemyBase _parent;
        Transform _target;
        float _stopDistance;
        #endregion

        public SeekState(EnemyBase parent, Transform target, float stopDistance)
        {
            _parent = parent;
            _target = target;
            _stopDistance = stopDistance;
        }

        public void Enter()
        {
            _parent.agent.stoppingDistance = _stopDistance;
            _parent.agent.isStopped = false;
            _parent.agent.SetDestination(_target.position);
            UpdateDestination();
        }

        public void Tick()
        {
            // update destination if target has moved more than STALE_EPS units
            float distance = (_parent.agent.destination - _target.position).magnitude;
            bool needToUpdateDestination = distance > STALE_EPS;
            if (needToUpdateDestination)
            {
                UpdateDestination();
            }

            // exit if agent has gotten to target
            bool InRange = _parent.agent.remainingDistance < _parent.agent.stoppingDistance + RANGE_EPS;
            if (InRange)
            {
                _parent.TriggerTransition();
            }
        }

        public void Exit() { }

        void UpdateDestination()
        {
            _parent.agent.SetDestination(_target.position);
        }

        const float STALE_EPS = .2f;
        const float RANGE_EPS = .1f;
    }
}
