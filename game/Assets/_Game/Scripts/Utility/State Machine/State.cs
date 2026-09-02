using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    public interface State
    {
        void Enter();
        void Tick();
        void Exit();
    }
}
