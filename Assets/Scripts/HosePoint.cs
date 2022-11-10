using Obi;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HosePoint : MonoBehaviour
{
    [SerializeField] private ObiCollider _obiCollider;
    [SerializeField] private Collider _collider;
    [SerializeField] private Collider _trigger;
    [Space]
    [SerializeField] private float _attachTime = .25f;
    [Space]
    [SerializeField] private LayerMask _surfaceMask;
    [SerializeField] private ParticleSystem _particle;

    [Header("Pick Up Properties")]
    [SerializeField] private ActionLoadingBar _pickUpLoadingBar;
    [SerializeField] private ParticleSystem _pickUpParticle;
    [SerializeField] private Transform _pickUpOutlineIndicator;
    [SerializeField] private Vector3 _progressBarOffset = new Vector3(1f, 2.5f, 0f);
    [SerializeField] private float _pickUpDelay = 1f;
    [Space]
    [SerializeField] private TargetPointer _targetPointer;

    private CharacterController _playerController;

    private Rigidbody _rigidBody;
    private ObiRigidbody _obiRigidbody;

    private IBlowable _blowableOjbect;
    private Transform _target;

    private bool _isDetached;
    private bool _pickingUp;
    private CharacterController _charController;

    private CoroutineItem _attachCor;

    private void OnDestroy()
    {
        if(_playerController != null)
        {
            _playerController.OnAttach -= Attach;
            _playerController.OnShocked -= Detach;
        }

        if (_blowableOjbect != null)
            _blowableOjbect.OnBlowOut -= Detach;
    }

    public void Init(CharacterController playerController)
    {
        if(_playerController != null)
        {
            _playerController.OnAttach -= Attach;
            _playerController.OnShocked -= Detach;
        }

        _playerController = playerController;
        _playerController.OnAttach += Attach;
        _playerController.OnShocked += Detach;

        _particle.Play();
        _obiCollider.sourceCollider = _collider;
        Attach(_playerController.GetAttachPoint(), true);
    }

    private void OnEnable()
    {
        GameManager.OnCompleted += Disable;
    }

    private void OnDisable()
    {
        GameManager.OnCompleted -= Disable;
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        _pickUpParticle.Stop();
        _pickUpOutlineIndicator.gameObject.SetActive(false);
        _collider.enabled = false;
        _trigger.enabled = false;
        _targetPointer.Deactivate(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if((1 << collision.gameObject.layer & _surfaceMask) != 0)
        {
            if (_isDetached == true && _pickUpParticle.isPlaying == false)
            {
                _pickUpParticle.transform.parent = transform;
                _pickUpParticle.transform.localPosition = Vector3.zero;
                _pickUpParticle.Play();
                _pickUpOutlineIndicator.gameObject.SetActive(true);
                _pickUpLoadingBar.transform.SetParent(transform, false);
                _pickUpLoadingBar.transform.position = _pickUpParticle.transform.position + _progressBarOffset;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If player enter to trigger - start pick uping
        if(_isDetached == true && other.TryGetComponent(out CharacterController cc) == true)
        {
            _charController = cc;

            if (cc.Shocked == true)
                return;

            PreparePickUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If player exit from trigger - cancel pick uping
        if (other.TryGetComponent(out CharacterController cc) == true)
        {
            if(_charController != null)
            {
                _charController = null;
                _pickUpLoadingBar.CancelLoading();
                _pickingUp = false;
            }
        }
    }

    private void Update()
    {
        if(_charController != null)
        {
            if (_charController.Shocked == false && _pickingUp == false && _isDetached == true)
            {
                PreparePickUp();
            }
        }

        if(_isDetached == true)
        {
            _pickUpParticle.transform.forward = _pickUpOutlineIndicator.forward = Vector3.up;
            _pickUpLoadingBar.transform.position = _pickUpParticle.transform.position + _progressBarOffset;
        }
    }

    private void PreparePickUp()
    {
        _pickUpLoadingBar.StartLoading(_pickUpDelay, PickUp);
        _pickingUp = true;
    }

    private bool Attach(Transform target, bool instantly = false)
    {
        if (_target == target)
            return false;

        if (_isDetached == true || instantly == true)
            _targetPointer.Deactivate(true);

        //Debug.Log("ATTACH");

        if (_obiRigidbody != null)
            Destroy(_obiRigidbody);

        if (_rigidBody != null)
            Destroy(_rigidBody);

        _charController = null;
        _isDetached = false;
        _pickUpParticle.transform.parent = null;
        _pickUpParticle.Stop();
        _pickUpOutlineIndicator.gameObject.SetActive(false);
        _pickUpLoadingBar.transform.SetParent(null, true);
        _pickUpLoadingBar.transform.position = _pickUpParticle.transform.position + _progressBarOffset;

        _target = target;
        transform.parent = target;
        transform.rotation = target.rotation;
        _collider.enabled = false;
        _trigger.enabled = false;

        this.DoAfterNextFixedFrameCoroutine(() =>
        {
            _obiCollider.sourceCollider = _collider;
        });

        if(instantly == true)
        {
            transform.localPosition = Vector3.zero;

            if (_blowableOjbect != null)
                _blowableOjbect.StartBlow();
        }
        else
        {
            _attachCor?.Stop();

            _attachCor = this.LerpCoroutine(
                time: _attachTime,
                from: transform.localPosition,
                to: Vector3.zero,
                action: a => transform.localPosition = a,
                onEnd: () =>
                {
                    if (_blowableOjbect != null)
                        _blowableOjbect.StartBlow();
                }
            );
        }

        return true;
    }

    private void Attach(IBlowable blowableObject)
    {
        if (Attach(blowableObject.GetAttachPoint()) == false) return;

        //Debug.Log("ATTACH BLOWABLE");
        _blowableOjbect = blowableObject;
        _blowableOjbect.OnBlowOut += Detach;
        _particle.Stop();
    }

    private void Detach()
    {
        if (_isDetached == true || (_blowableOjbect != null && _blowableOjbect.State == BlowState.Blowing))
            return;

        //Debug.Log("DETACH");

        if(_blowableOjbect != null)
        {
            _blowableOjbect.OnBlowOut -= Detach;
            _blowableOjbect = null;
        }

        this.DoAfterNextFixedFrameCoroutine(() =>
        {
            if (_rigidBody == null)
                _rigidBody = gameObject.AddComponent<Rigidbody>();

            if (_obiRigidbody == null)
                _obiRigidbody = gameObject.AddComponent<ObiRigidbody>();

            _obiCollider.sourceCollider = _collider;
            _isDetached = true;
        });

        _target = null;
        transform.parent = null;
        transform.localScale = Vector3.one;
        _collider.enabled = true;
        _trigger.enabled = true;
        _particle.Play();

        _targetPointer.Activate();
    }

    private void PickUp()
    {
        Attach(_playerController.GetAttachPoint());
        _pickingUp = false;
    }
}
