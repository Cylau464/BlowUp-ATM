using UnityEngine;
using UnityEngine.AI;
using States;
using States.Enemy;
using System;
using Obi;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour, IBlowable
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyAnimationController _animationController;
    [SerializeField] private FieldOfView _fieldOfView;
    [Space]
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Collider _collider;
    [SerializeField] private ObiRigidbody _obiRigidBody;
    [SerializeField] private ObiCollider _obiCollider;

    [Header("Patroling")]
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _patrolingSpeed = 2f;
    [SerializeField] private float _minWaitTime = 1f;
    [SerializeField] private float _maxWaitTime = 3f;
    private int _curPointIndex;
    private bool _isWaiting;

    [Header("Chasing")]
    [SerializeField] private AgroSign _agroSign;
    [SerializeField] private float _chasingSpeed = 8f;
    [SerializeField] private float _scanRadius = 4f;
    [SerializeField] private float _scanAngle = 30f;
    [SerializeField] private float _posCheckFrequency = .2f;
    [SerializeField] private float _chasingMaxDistance = 10f;
    [SerializeField] private float _chaseDelay = 2f;
    private float _curPosCheckDelay;
    private float _curChaseDelay;
    private Vector3 _startChasingPos;

    [Space]
    [SerializeField] private Weapon _weapon;
    [Header("Aiming")]
    [SerializeField] private float _aimDistance = 5f;
    [SerializeField] private float _aimingRotationSpeed = 180f;
    [SerializeField] private float _minAimingTime = .5f;
    [SerializeField] private float _maxAimingTime = 1.5f;
    private float _aimingTime;

    [Header("Shoting")]
    [SerializeField] private float _delayAfterShot = 1f;
    public float CurDelayAfterShot { get; private set; }

    [Header("Blowing")]
    [SerializeField] private Transform _hoseAttachPoint;
    [SerializeField] private ParticleSystem _blowingOffParticle;
    [SerializeField] private ActionLoadingBar _blowPrepareLoadingBar;
    [Space(2f)]
    [SerializeField] private float _blowTime = 2f;
    [SerializeField] private float _flyingForce = 1f;
    [SerializeField] private float _flyingTorque = 1f;
    [Space(2f)]
    [SerializeField] private float _blowOffTime = 2f;
    [SerializeField] private float _blowOffForce = 5f;
    [SerializeField] private float _blowOffTorque = 1f;
    [SerializeField] private Color _blowOffColor = Color.grey;
    [Space(2f)]
    [SerializeField] private float _ragdollDisableDelay = 5f;
    [SerializeField] private LayerMask _blowOutMask;
    [SerializeField] private Outline _outline;

    private CoroutineItem _blowPrepareCor;

    private Vector3 _torqueDirection;

    private StateMachine _stateMachine;
    private MaterialPropertyBlock _propertyBlock;
    private Transform _shotTarget;

    private const string materialColorName = "_BaseColor";

    public BlowState State { get; private set; }

    public Action OnStartBlow { get; set; }
    public Action OnBlowOut { get; set; }

    private void Start()
    {
        _stateMachine = new StateMachine(this);
        _stateMachine.SetState<Patrol>();
        _stateMachine.OnStateSwitched += OnStateSwitched;
        _fieldOfView.ViewAngle = _scanAngle;
        _fieldOfView.ViewRadius = _scanRadius;

        _propertyBlock = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_propertyBlock);
        
        _weapon.Init(_collider);
    }

    private void OnDestroy()
    {
        _stateMachine.OnStateSwitched -= OnStateSwitched;

        if(_animationController != null)
            _animationController.OnShot -= Shot;
    }

    private void Update()
    {
        _stateMachine.Update();
        _animationController.SetSpeed(_agent.velocity.magnitude / _chasingSpeed);

        _curPosCheckDelay -= Time.deltaTime;
        _curChaseDelay -= Time.deltaTime;
        CurDelayAfterShot -= Time.deltaTime;
    }

    private void OnStateSwitched()
    {
        CancelInvoke(nameof(MoveToNextPoint));
    }

    public void MoveToNextPoint()
    {
        if (_fieldOfView.IsHided == true)
            _fieldOfView.Show();

        _isWaiting = false;
        _agent.speed = _patrolingSpeed;
        _agent.destination = _patrolPoints[_curPointIndex].position;

        if (++_curPointIndex >= _patrolPoints.Length)
            _curPointIndex = 0;
    }

    public void Wait()
    {
        _isWaiting = true;
        Invoke(nameof(MoveToNextPoint), Random.Range(_minWaitTime, _maxWaitTime));
    }

    public bool IsDestinationReached()
    {
        return _isWaiting == false
            && _agent.enabled == true
            && _agent.pathPending == false
            && _agent.remainingDistance <= _agent.stoppingDistance
            && (_agent.hasPath || _agent.velocity.sqrMagnitude == 0f);
    }

    public bool CheckTarget()
    {
        bool caughtUp = _curChaseDelay <= 0f && _fieldOfView.VisibleTargets.Count > 0;

        if (caughtUp == true)
        {
            if(_fieldOfView.VisibleTargets[0].TryGetComponent(out CharacterController cc) == true)
            {
                if(cc.Shocked == true)
                    caughtUp = false;
                else
                    _agroSign.Show();
            }

            _fieldOfView.Hide();
        }

        return caughtUp;
    }

    public Transform GetTarget()
    {
        _startChasingPos = transform.position;

        return _fieldOfView.VisibleTargets[0];
    }

    public ChaseStatus ChaseTarget(Transform target)
    {
        if (Vector3.Distance(target.position, transform.position) <= _aimDistance)
            return ChaseStatus.CuaghtUp;

        if (Vector3.Distance(_startChasingPos, transform.position) >= _chasingMaxDistance)
            return ChaseStatus.Abort;

        if (_curPosCheckDelay > 0f)
            return ChaseStatus.Chase;

        _curPosCheckDelay = _posCheckFrequency;
        _agent.speed = _chasingSpeed;
        _agent.destination = target.position;
        
        return ChaseStatus.Chase;
    }

    public void SetChaseDelay()
    {
        _curChaseDelay = _chaseDelay;
    }

    public void StartAim()
    {
        _animationController.Aim();
        _agent.destination = transform.position;
        _aimingTime = Random.Range(_minAimingTime, _maxAimingTime);
    }

    public bool Aiming(Transform target)
    {
        _aimingTime -= Time.deltaTime;
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion rotateTo = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, _aimingRotationSpeed * Time.deltaTime);

        return _aimingTime <= 0f;
    }

    public void StartShot(Transform target, Action action)
    {
        _shotTarget = target;
        _animationController.StartShot();
        _animationController.OnShot += Shot;
        _animationController.OnShot += action;
    }

    private void Shot()
    {
        CurDelayAfterShot = _delayAfterShot;
        _curChaseDelay = _chaseDelay + _delayAfterShot;
        _weapon.Shot((_shotTarget.position - transform.position).normalized);
        _animationController.OnShot -= Shot;
        _shotTarget = null;
    }

    #region Blowing
    public void PrepareBlow(float duration, Action onComplete)
    {
        _blowPrepareLoadingBar.StartLoading(duration, onComplete);
    }

    public void CancelPrepare()
    {
        _blowPrepareLoadingBar.CancelLoading();
    }

    public Transform GetAttachPoint()
    {
        return _hoseAttachPoint;
    }

    public void StartBlow()
    {
        State = BlowState.Blowing;
        _fieldOfView.Hide();
        _torqueDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        _agent.enabled = false;
        _rigidBody.isKinematic = true;
        //_rigidBody.useGravity = false;
        _collider.enabled = false;
        _obiRigidBody.kinematicForParticles = true;
        _obiCollider.enabled = false;
        _animationController.StartBlow();

        Vector3 startScale = transform.localScale;
        float shapeStart = _renderer.GetBlendShapeWeight(0);
        float shapeTarget = 100f;

        StopAllCoroutines();

        this.LerpCoroutine(
            time: _blowTime,
            from: 0f,
            to: 1f,
            action: a =>
            {
                _renderer.SetBlendShapeWeight(0, Mathf.Lerp(shapeStart, shapeTarget, a));
                _animationController.Blowing(Vector3.up * _flyingForce, _torqueDirection * _flyingTorque, ForceMode.Acceleration);
            },
            onEnd: () => BlowOut(),
            settings: new CoroutineTemplate.Settings(useFixedDelta: true)
        );

        OnStartBlow?.Invoke();

        Dictionary<string, object> eventInfo = new Dictionary<string, object>()
        {
            { "level_id", GameManager.Instance.levelsData.idLevel }
        };
        apps.EventsLogger.CustomEvent("enemy_blowed", eventInfo);
    }

    public void BlowOut()
    {
        _outline.enabled = false;
        State = BlowState.Blowed;
        OnBlowOut?.Invoke();
        _blowingOffParticle.Play();
        _animationController.BlowOut();
        SetLayerToChilds(transform, (int) (Mathf.Log(_blowOutMask) / Mathf.Log(2)));

        Vector3 startScale = transform.localScale;
        Color startColor = _renderer.sharedMaterial.GetColor(materialColorName);
        float shapeStart = _renderer.GetBlendShapeWeight(0);
        float shapeTarget = 0f;

        this.LerpCoroutine(
            time: _blowOffTime,
            from: 0f,
            to: 1f,
            action: a =>
            {
                //_rigidBody.AddTorque(_hoseAttachPoint.forward * _blowOffTorque);
                _propertyBlock.SetColor(materialColorName, Color.Lerp(startColor, _blowOffColor, a));
                _renderer.SetPropertyBlock(_propertyBlock);
                _animationController.BlowingOff(-_hoseAttachPoint.forward * _blowOffForce, _hoseAttachPoint.position, ForceMode.Acceleration);
                _renderer.SetBlendShapeWeight(0, Mathf.Lerp(shapeStart, shapeTarget, a));
            },
            onEnd: () => _blowingOffParticle.Stop(),
            settings: new CoroutineTemplate.Settings(useFixedDelta: true)
        );
    }

    private void SetLayerToChilds(Transform parent, int layer)
    {
        parent.gameObject.layer = layer;

        foreach(Transform child in parent)
        {
            SetLayerToChilds(child, layer);
        }
    }
    #endregion
}