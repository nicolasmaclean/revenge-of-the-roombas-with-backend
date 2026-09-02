using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Utility;
using Game.Player;

namespace Game.Enemy
{
    public sealed class EnemyWalla : EnemyBase
    {
        #region states
        State _idle;
        State _softSeek;
        #endregion

        #region serialized variables
        [Header("Controls")]
        [SerializeField]
        float _waitTime = 1f;
        
        [SerializeField]
        [Tooltip("The range this enemy should attempt to maintain from the player.")]
        Vector2 _range = new Vector2(4, 8);
        #endregion

        void Start()
        {
            _softSeek = new States.MaintainRangeState(this, PlayerController.Instance.transform, _range);
            _idle     = new States.WaitState(this, _waitTime);

            ChangeState(_softSeek);
        }

        public override void TriggerTransition()
        {
            if (CurrentState == _softSeek)
            {
                ChangeState(_idle);
            }
            else
            {
                ChangeState(_softSeek);
            }
        }
        public override void Kill()
        {
            base.Kill();
            agent.enabled = false;
        }
    }
}
