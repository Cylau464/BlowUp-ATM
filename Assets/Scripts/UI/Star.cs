using main.level;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private int _fillParamID;
    protected int _index;

    public void Init(int starIndex)
    {
        _index = starIndex;
    }

    public virtual void Fill()
    {
        _fillParamID = Animator.StringToHash("fill");
        _animator.SetTrigger(_fillParamID);
    }
}
