﻿using System;
using WheelOfFortune.Constants;

namespace WheelOfFortune.Single
{
    public class WheelSignal
    {
        public Action TriggerStateMachine;
        public Action<WheelStateName> TriggerStateMachineWithName;

        public void Reset()
        {
            TriggerStateMachine = null;
            TriggerStateMachineWithName = null;
        }
    }
}