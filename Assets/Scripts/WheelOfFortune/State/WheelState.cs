using System;
using System.Collections.Generic;
using WheelOfFortune.Constants;

namespace WheelOfFortune.State
{
    public class WheelState
    {
        public readonly WheelStateName StateName;
        private Action _enterAction;
        private Action _exitAction;
        private Dictionary<WheelState, Func<bool>> _exitConditions;

        public WheelState(WheelStateName stateName)
        {
            StateName = stateName;
        }

        public void SetState(Action enterAction = null, Action exitAction = null, Dictionary<WheelState, Func<bool>> exitConditions = null)
        {
            _enterAction = enterAction;
            _exitAction = exitAction;
            _exitConditions = exitConditions;
        }

        public void Enter()
        {
            _enterAction?.Invoke();
        }

        public WheelState CheckExit()
        {
            if (_exitConditions == null) return null;
            
            foreach (var (state, condition) in _exitConditions)
            {
                if (condition()) return state;
            }
            
            return null;
        }

        public void Exit()
        {
            _exitAction?.Invoke();
        }
    }
}