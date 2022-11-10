using UnityEngine;
using System;

public interface IBlowable
{
    public BlowState State { get; }

    public Action OnStartBlow { get; set; }
    public Action OnBlowOut { get; set; }

    public Transform GetAttachPoint();
    public void PrepareBlow(float duration, Action onComplete);
    public void CancelPrepare();
    public void StartBlow();
    public void BlowOut();
}

public enum BlowState { NotBlow, Blowing, Blowed }