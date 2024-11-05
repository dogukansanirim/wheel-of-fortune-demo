using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using WheelOfFortune.Constants;
using WheelOfFortune.Single;

namespace WheelOfFortune.State
{
    public class WheelStateMachine
    {
        private readonly WheelState _initState = new(WheelStateName.Init);
        private readonly WheelState _reloadState = new(WheelStateName.Reload);
        private readonly WheelState _idleState = new(WheelStateName.Idle);
        private readonly WheelState _spinState = new(WheelStateName.Spin);
        private readonly WheelState _rewardState = new(WheelStateName.Reward);
        private readonly WheelState _bombState = new(WheelStateName.Bomb);
        private readonly WheelState _finishState = new(WheelStateName.Finish);
        private readonly WheelState _emptyState = new(WheelStateName.Empty);
        
        private bool _isStateChanging;
        private WheelState _currentState;

        public WheelStateMachine()
        {
            WheelSingleton.Instance.Signal.TriggerStateMachine += Trigger;
            WheelSingleton.Instance.Signal.TriggerStateMachineWithName += TriggerWithName;
            SetStateMachine();
            ChangeState(_initState);
        }

        private void SetStateMachine()
        {
            _initState.SetState(
                enterAction: () =>
                {
                    WheelSingleton.Instance.WheelUIController.StartFadeOut();
                },
                exitConditions: new Dictionary<WheelState, Func<bool>>
                {
                    [_reloadState] = () => WheelSingleton.Instance.WheelUIController.IsFadeInOutActive == false
                }
            );
            
            _reloadState.SetState(
                enterAction: () =>
                {
                    WheelSingleton.Instance.WheelUIController.ReloadWheel();
                },
                exitConditions: new Dictionary<WheelState, Func<bool>>
                {
                    [_idleState] = () => WheelSingleton.Instance.WheelUIController.IsWheelReloaded
                }
            );
            
            _idleState.SetState(
                enterAction: () =>
                {
                    WheelSingleton.Instance.WheelUIController.SetSpinButtonInteractable(true);
                    WheelSingleton.Instance.WheelUIController.SetExitButtonInteractable(true);
                },
                exitAction: () =>
                {
                    WheelSingleton.Instance.WheelUIController.SetSpinButtonInteractable(false);
                    WheelSingleton.Instance.WheelUIController.SetExitButtonInteractable(false);
                },
                exitConditions: new Dictionary<WheelState, Func<bool>>
                {
                    [_finishState] = () => WheelSingleton.Instance.WheelUIController.IsExitBtnClicked,
                    [_spinState] = () => WheelSingleton.Instance.WheelUIController.IsWheelSpinning,
                }
            );
            
            _spinState.SetState(
                exitConditions: new Dictionary<WheelState, Func<bool>>
                {
                    [_rewardState] = () => !WheelSingleton.Instance.WheelUIController.IsWheelSpinning && !WheelSingleton.Instance.WheelUIController.IsCurrentRewardBomb,
                    [_bombState] = () => !WheelSingleton.Instance.WheelUIController.IsWheelSpinning && WheelSingleton.Instance.WheelUIController.IsCurrentRewardBomb,
                }
            );
            
            _rewardState.SetState(
                enterAction: () =>
                {
                    WheelSingleton.Instance.RewardController.ActivateCurrentReward(WheelSingleton.Instance.WheelUIController.CurrentRewardValue, WheelSingleton.Instance.WheelUIController.CurrentReward, WheelSingleton.Instance.WheelUIController.SliceRewardIconRectTransform);
                },
                exitConditions: new Dictionary<WheelState, Func<bool>>
                {
                    [_reloadState] = () => !WheelSingleton.Instance.RewardController.IsRewardActive && !WheelSingleton.Instance.WheelUIController.IsLastSpin,
                    [_finishState] = () => !WheelSingleton.Instance.RewardController.IsRewardActive && WheelSingleton.Instance.WheelUIController.IsLastSpin,
                }
            );
            
            _bombState.SetState(
                enterAction: () =>
                {
                    WheelSingleton.Instance.WheelUIController.StartBombAnimation();
                },
                exitAction: () =>
                {
                    SceneManager.LoadScene(SceneName.Wheel);
                },
                exitConditions: new Dictionary<WheelState, Func<bool>>
                {
                    [_emptyState] = () => WheelSingleton.Instance.WheelUIController.IsBombAnimationOver,
                }
            );
            
            _finishState.SetState(
                enterAction: () =>
                {
                    WheelSingleton.Instance.WheelUIController.StartFadeIn();
                },
                exitAction: () =>
                {
                    SceneManager.LoadScene(SceneName.Wheel);
                },
                exitConditions: new Dictionary<WheelState, Func<bool>>
                {
                    [_emptyState] = () => WheelSingleton.Instance.WheelUIController.IsFadeInOutActive == false
                }
            );
        }

        private void ChangeState(WheelState state)
        {
            _isStateChanging = true;
            _currentState?.Exit();
            _currentState = state;
            _currentState?.Enter();
            _isStateChanging = false;
            Trigger();
        }

        private void Trigger()
        {
            if (_isStateChanging || _currentState == null) return;
            WheelState nextState = _currentState.CheckExit();
            if (nextState != null) ChangeState(nextState);
        }
        
        private void TriggerWithName(WheelStateName name)
        {
            if (_isStateChanging || _currentState == null || _currentState.StateName != name) return;
            WheelState nextState = _currentState.CheckExit();
            if (nextState != null) ChangeState(nextState);
        }

        public void Reset()
        {
            WheelSingleton.Instance.Signal.TriggerStateMachine -= Trigger;
            WheelSingleton.Instance.Signal.TriggerStateMachineWithName -= TriggerWithName;
        }
    }
}