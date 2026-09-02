using UnityEngine;
using Game.Utility;

namespace Game.Enemy.States
{
    public class WaitState : State
    {
        #region public variables
        public float WaitTime;
        #endregion

        #region private variables
        EnemyBase _parent;
        float _startTime;
        #endregion

        public WaitState(EnemyBase parent, float waitTime)
        {
            _parent = parent;
            WaitTime = waitTime;
        }

        public void Enter()
        {
            _startTime = Time.time;
            _parent.agent.isStopped = true;
        }

        public void Tick()
        {
            float elapsedTime = Time.time - _startTime;
            if (elapsedTime > WaitTime)
            {
                // if wait time has elapsed, change to passed state
                _parent.TriggerTransition();
            }
        }

        public void Exit() { }
    }
}
