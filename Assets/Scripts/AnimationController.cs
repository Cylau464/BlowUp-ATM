using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    [SerializeField] protected RagdollHelper _ragdollHelper;

    protected int _speedParamID;

    protected void Awake()
    {
        _speedParamID = Animator.StringToHash("speed");
    }

    public void SetSpeed(float speed)
    {
        //if (speed < .01f) speed = 0f;

        _animator.SetFloat(_speedParamID, speed);
    }
}
