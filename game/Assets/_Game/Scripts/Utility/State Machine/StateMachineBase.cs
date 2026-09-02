using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    public abstract class StateMachineBase : MonoBehaviour
    {
        #region public variables
        public State CurrentState { get; set; }
        #endregion

        #region private variables
        bool _inTransition = false;
        #endregion

        public void ChangeState(State newState)
        {
            // checking can change state
            if (CurrentState == newState || _inTransition)
                return;
            
            ChangeStateRoutine(newState);
        }

        void ChangeStateRoutine(State newState)
        {
            _inTransition = true;

            // exiting the current state
            if (CurrentState != null)
                CurrentState.Exit();
            
            CurrentState = newState;

            // entering the new state
            if (CurrentState != null)
                CurrentState.Enter();
            
            _inTransition = false;
        }

        public void Update()
        {
            if (CurrentState != null && !_inTransition)
                CurrentState.Tick();
        }

    }
}

