using System;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Collider _collider;
    [SerializeField] private Collider _trigger;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _moveToCollectorTime = 1f;
    [SerializeField] private int _price = 1;
    public int Price => _price;
    [SerializeField] private float _pickUpDelay = .5f;
    [SerializeField] private float _scatterPickUpDelay = 2f;
    [Space]
    [SerializeField] private float _inertia = 1f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _maxOffset = 2f;
    [SerializeField] private float _deltaThreshold = .001f;

    private bool _pickedUp;
    private bool _waitForShockEnd;
    private CharacterController _characterController;
    private Vector3 _parentLastPosition;
    private Quaternion _parentLastRotation;
    private Vector3 _targetLocalPos;

    private void OnDisable()
    {
        _trigger.enabled = false;
        CancelInvoke(nameof(ActivateTrigger));
    }

    private void OnEnable()
    {
        Invoke(nameof(ActivateTrigger), _pickUpDelay);
    }

    private void Update()
    {
        if(_waitForShockEnd == true && _characterController.Shocked == false)
        {
            PickUp();
            _waitForShockEnd = false;
        }

        if (_pickedUp == true)
        {
            //Swaying();
        }
    }

    private void Swaying()
    {
        Vector3 delta = transform.InverseTransformPoint(_parentLastPosition) - transform.InverseTransformPoint(transform.parent.position);
        bool returnToTarget = delta.sqrMagnitude < _deltaThreshold;
        //delta = transform.InverseTransformDirection(delta);
        delta.y = 0f;
        delta *= Mathf.Lerp(1f, _speed, transform.localPosition.y / 8.5f);
        _parentLastPosition = transform.parent.position;

        Vector3 localPos = transform.localPosition;
        localPos.y = 0f;
        float minOffset = Mathf.Abs(_targetLocalPos.x); // new Vector3(_targetLocalPos.x, 0f, _targetLocalPos.z).magnitude;
        //Vector3 targetPosition = new Vector3(_targetLocalPos.x, 0f, _targetLocalPos.z);
        Vector3 clampedPosition = Vector3.ClampMagnitude(localPos + delta, Mathf.Lerp(minOffset, _maxOffset + minOffset, transform.localPosition.y / 8.5f));
        //Vector3 newPosition = localPos + delta;
        //float offset = Mathf.Lerp(minOffset, _maxOffset + minOffset, transform.localPosition.y / 8.5f);

        //if (Vector3.Distance(newPosition, targetPosition) > offset)
        //{
        //    newPosition =  (newPosition - targetPosition).normalized * offset;
        //}

        clampedPosition.y += _targetLocalPos.y;
        transform.localPosition = clampedPosition;

        if (returnToTarget == true)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _targetLocalPos, Time.deltaTime * _inertia);
    }

    private void PickUp()
    {
        _rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        _rigidBody.interpolation = RigidbodyInterpolation.None;
        _rigidBody.isKinematic = true;
        _collider.enabled = false;
        _trigger.enabled = false;
        _characterController.MoneyHolder.PickUp(this);
        _pickedUp = true;
        _parentLastPosition = transform.parent.position;
        _parentLastRotation = transform.parent.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & _playerLayer) != 0)
        {
            _characterController = other.GetComponent<CharacterController>();

            if (_characterController.Shocked == false)
                PickUp();
            else
                _waitForShockEnd = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & _playerLayer) != 0)
        {
            if(_characterController != null && _characterController.Shocked == true)
            {
                _waitForShockEnd = false;
                _characterController = null;
            }
        }
    }

    public void AddExplosionForce(Vector3 explosionPoint, float force, float radius)
    {
        _rigidBody.AddExplosionForce(force, explosionPoint, radius, 1f);
    }

    private void ActivateTrigger()
    {
        _trigger.enabled = true;
    }

    public void MoveToCollector(Transform collector, Vector3 localPos, Action<int> onEndAction = null, AnimationCurve verticalMoveCurve = null, AnimationCurve horizontalMoveCurve = null, bool inactiveAfterMove = false)
    {
        if (onEndAction != null)
            _pickedUp = false;

        _targetLocalPos = localPos;
        transform.parent = collector;
        Vector3 startLocalPos = transform.localPosition;
        Quaternion startLocalRot = transform.localRotation;
        Vector3 newPos;

        this.LerpCoroutine(
            time: _moveToCollectorTime,
            from: 0f,
            to: 1f,
            action: a =>
            {
                newPos = Vector3.Lerp(startLocalPos, localPos, a);

                if(verticalMoveCurve != null)
                {
                    newPos.z += horizontalMoveCurve.Evaluate(a);
                    newPos.y += verticalMoveCurve.Evaluate(a);
                }

                transform.localPosition = newPos;
                transform.localRotation = Quaternion.Lerp(startLocalRot, Quaternion.identity, a);
            },
            onEnd: () =>
            {
                if(onEndAction != null)
                    onEndAction(_price);

                if (inactiveAfterMove == true)
                    gameObject.SetActive(false);
            }
        );
    }

    public Vector3 GetBoundsSize()
    {
        Quaternion curRot = transform.rotation;
        transform.rotation = Quaternion.identity;
        Vector3 size = _renderer.bounds.size;
        transform.rotation = curRot;

        return size;
    }

    public void Scatter(float force, Vector3 explosionDirection)
    {
        transform.parent = null;
        Invoke(nameof(ActivateTrigger), _scatterPickUpDelay);
        _collider.enabled = true;
        _rigidBody.isKinematic = false;
        _rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidBody.AddForce(explosionDirection * force, ForceMode.Impulse);
        _pickedUp = false;
    }
}
