using Game.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy.States
{
    public class MaintainRangeState : State
    {
        #region instance variables
        EnemyBase _parent;
        Transform _target;
        Vector2 _range;
        #endregion

        #region constants
        float DISTANCE_BUFFER = .1f;
        float MIN_ANGLE_RANGE = -60f;
        float MAX_ANGLE_RANGE = 60f;
        #endregion

        public MaintainRangeState(EnemyBase parent, Transform target, Vector2 range)
        {
            _parent = parent;
            _target = target;
            _range = range;
        }

        public void Enter()
        {
            // calculating the distance from the agent to target
            float distanceFromTarget = (_target.position - _parent.transform.position).magnitude;
            Vector3 destination;

            bool agentIsTooFar = distanceFromTarget > _range.y;
            if (agentIsTooFar)
            {
                // move to target (but not too close)
                destination = _target.transform.position;
                _parent.agent.stoppingDistance = _range.x;
            }
            else
            {
                Vector3 newDirection = (_parent.transform.position - _target.position).normalized;
                Quaternion rotation;
                float newDistance;

                bool agentIsTooNear = distanceFromTarget < _range.x;
                if (agentIsTooNear)
                {
                    // move away from target
                    rotation = Quaternion.Euler(0, Random.Range(MIN_ANGLE_RANGE, MAX_ANGLE_RANGE), 0);
                    newDistance = _range.y - distanceFromTarget;
                }
                else
                {
                    // stay in the meso-sphere from the target
                    rotation = Quaternion.Euler(0, Random.Range(MIN_ANGLE_RANGE, MAX_ANGLE_RANGE), 0);
                    newDistance = (_range.x + _range.y) / 2;
                }

                // rotate current direction vector by rotation
                newDirection = rotation * newDirection;

                destination = _parent.transform.position + newDirection * newDistance;
                _parent.agent.stoppingDistance = 0;
            }

            _parent.agent.SetDestination(destination);
            _parent.agent.isStopped = false;
        }

        public void Tick()
        {
            bool atDestination = _parent.agent.remainingDistance < _parent.agent.stoppingDistance + DISTANCE_BUFFER;
            if (atDestination)
            {
                // if destination has been reached, change state to passed state
                _parent.TriggerTransition();
            }
        }

        public void Exit()
        {
            _parent.agent.isStopped = true;
        }
    }
}
