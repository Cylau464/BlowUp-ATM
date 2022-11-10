using System;
using UnityEngine;

public class EnemyAnimationController : AnimationController
{
    private int _aimParamID;
    private int _shotParamID;

    public Action OnShot;

    private new void Awake()
    {
        base.Awake();

        _aimParamID = Animator.StringToHash("aim");
        _shotParamID = Animator.StringToHash("shot");
    }

    public void Aim()
    {
        _animator.SetTrigger(_aimParamID);
    }

    public void StartShot()
    {
        _animator.SetTrigger(_shotParamID);
    }

    private void Shot()
    {
        OnShot?.Invoke();
    }

    public void StartBlow()
    {
        _ragdollHelper.StartBlow();
    }

    public void Blowing(Vector3 force, Vector3 torque, ForceMode mode)
    {
        _ragdollHelper.Blowing(force, torque, mode);
    }

    public void BlowOut()
    {
        _ragdollHelper.BlowOut();
    }

    public void BlowingOff(Vector3 force, Vector3 position, ForceMode mode)
    {
        _ragdollHelper.BlowingOff(force, position, mode);
    }
}