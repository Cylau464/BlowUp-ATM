using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Collider _collider;
    [SerializeField] private float _maxMoveSpeed = 6f;
    [SerializeField] private float _acceleration = .1f;
    [SerializeField] private float _rotateSpeed = 1f;

    [Header("Hose")]
    [SerializeField] private Transform _attachPoint;
    [SerializeField] private float _attachHosePrepareTime = .5f;
    private IBlowable _attachableBlowable;

    [Header("Money")]
    [SerializeField] private MoneyHolder _moneyHolder;
    public MoneyHolder MoneyHolder => _moneyHolder;

    [Header("Shock")]
    [SerializeField] private ParticleSystem _shockParticle;

    private bool _hoseAttached = true;
    private float _moveSpeed;
    private Vector3 _moveDirection;

    public bool Shocked { get; private set; }

    public Action<IBlowable> OnAttach { get; set; }
    public Action OnShocked { get; set; }
    public Action OnReachedLeaveTarget { get; set; }

    private void Start()
    {
        this.DoAfterNextFrameCoroutine(() =>
            _animationController.CarryHose(true));
    }

    private void Update()
    {
        if (Shocked == false)
            _moveDirection = new Vector3(Joystick.Instance.Direction.x, 0f, Joystick.Instance.Direction.y);
        else
            _moveDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if(GameManager.isPlaying == true)
        {
            float magnitude = _moveDirection.magnitude;
            float force = Time.fixedDeltaTime * _acceleration;

            if(magnitude > 0f)
            {
                _moveSpeed += force;
                _moveSpeed = Mathf.Clamp(_moveSpeed, 0f, _maxMoveSpeed * magnitude);
                _rigidBody.velocity = Vector3.ClampMagnitude(_moveDirection.normalized * _moveSpeed, _maxMoveSpeed);
            }
            else
            {
                _moveSpeed -= force;
                _moveSpeed = Mathf.Clamp(_moveSpeed, 0f, _maxMoveSpeed);
                _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity.normalized * _moveSpeed, _maxMoveSpeed);
            }

            _animationController.SetSpeed(_moveSpeed / _maxMoveSpeed);

            if (Joystick.Instance.IsDragging == true && _rigidBody.velocity.sqrMagnitude > 0f)
            {
                Quaternion rotateTo = Quaternion.LookRotation(_moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, _rotateSpeed * Time.fixedDeltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IBlowable blowable) == true
        || (blowable = other.GetComponentInParent<IBlowable>()) != null)
        {
            if (_hoseAttached == true && blowable.State == BlowState.NotBlow)
            {
                if(_attachableBlowable != null && _attachableBlowable != blowable)
                    _attachableBlowable.CancelPrepare();

                _attachableBlowable = blowable;
                _attachableBlowable.PrepareBlow(_attachHosePrepareTime, AttachHose);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IBlowable blowable) == true
        || (blowable = other.GetComponentInParent<IBlowable>()) != null)
        {
            if (_hoseAttached == true)
            {
                if(_attachableBlowable != null && _attachableBlowable == blowable)
                {
                    _attachableBlowable.CancelPrepare();
                    _attachableBlowable = null;
                }
            }
        }
    }

    private void AttachHose()
    {
        _hoseAttached = false;
        _animationController.CarryHose(_hoseAttached);
        OnAttach?.Invoke(_attachableBlowable);
        _attachableBlowable = null;
    }

    public Transform GetAttachPoint()
    {
        _hoseAttached = true;
        _animationController.CarryHose(_hoseAttached);

        return _attachPoint;
    }

    public void Shock(float duration, Vector3 direction)
    {
        _animationController.Shock(duration);
        _hoseAttached = false;
        Shocked = true;
        _animationController.CarryHose(_hoseAttached);
        _animationController.OnGotUp += ShockEnd;
        _shockParticle.Play();
        _moneyHolder.ScatterMoney(direction);
        Invoke(nameof(StopShockParticle), duration);

        OnShocked?.Invoke();
    }

    private void StopShockParticle()
    {
        _shockParticle.Stop();
    }

    private void ShockEnd()
    {
        Shocked = false;
        _animationController.OnGotUp -= ShockEnd;
    }

    public void Leave(Transform leaveTarget)
    {
        Joystick.Instance.Disable();
        _collider.enabled = false;
        _rigidBody.isKinematic = true;
        _rigidBody.interpolation = RigidbodyInterpolation.None;
        transform.parent = leaveTarget;

        Vector3 startPos = transform.position;
        Vector3 direction = (leaveTarget.position - transform.position).normalized;
        direction.y = 0f;
        Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
        float moveTime = .5f;

        this.LerpCoroutine(
            time: moveTime,
            from: 0f,
            to: 1f,
            action: a =>
            {
                transform.position = Vector3.Lerp(startPos, leaveTarget.position, a);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 360f * Time.deltaTime / moveTime);
                _animationController.SetSpeed(_maxMoveSpeed);
            },
            onEnd: () =>
            {
                direction = (leaveTarget.position - transform.position).normalized;
                direction.y = 0f;
                transform.rotation = Quaternion.identity;//Quaternion.LookRotation(direction, Vector3.up);
                OnReachedLeaveTarget?.Invoke();
            }
        );
    }
}
