using System;
using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    private int _carryHoseParamID;
    private int _shockParamID;
    private int _getUpParamID;

    public Action OnGotUp;

    private new void Awake()
    {
        base.Awake();

        _carryHoseParamID = Animator.StringToHash("carry_hose");
        _shockParamID = Animator.StringToHash("shock");
        _getUpParamID = Animator.StringToHash("get_up");
    }

    public void CarryHose(bool carry)
    { 
        _animator.SetBool(_carryHoseParamID, carry);
    }

    public void Shock(float duration)
    {
        _animator.SetTrigger(_shockParamID);
        Invoke(nameof(GetUp), duration);
    }

    private void GetUp()
    {
        _animator.SetTrigger(_getUpParamID);
    }

    private void GotUp()
    {
        OnGotUp?.Invoke();
    }
}