using System.Collections;
using UnityEngine;
using Obi;
using System;

public class Hose : MonoBehaviour
{
    [SerializeField] private ObiSolver _solver;
    [SerializeField] private HosePoint _hosePoint;
    [SerializeField] private float _attachTime = .25f;

    [Header("Length Control")]
    [SerializeField] private float _maxLength = 50f;
    [SerializeField] private float _increaseLength = 2f;
    [SerializeField] private float _decreaseLength = 1f;
    [SerializeField] private float _changeLengthTime = .25f;
    private float _minLength;
    private float _curLength;

    [Header("Blueprint Settings")]
    [SerializeField] private float _resolution = .1f;
    [SerializeField] private float _particleMass = 2f;
    [SerializeField] private int _particlePoolSize = 100;

    [SerializeField] private ObiRope _rope;
    [SerializeField] private ObiRopeExtrudedRenderer _ropeRenderer;
    [SerializeField] private ObiRopeCursor _cursor;
    [SerializeField] private HosePump _hosePump;

    private ObiRopeBlueprint _blueprint;
    private ObiParticleAttachment[] _particleAttachments;

    private CharacterController _playerController;
    private Transform _startPoint;
    private ObiCollider _startObiCollider;

    private CoroutineItem _changeLengthCor;
    private enum ChangeStatus { Idle, Increase, Decrease }
    private ChangeStatus _changeStatus;

    private IBlowable _blowableObject;
    private bool _ropeGenerated;

    public static Hose Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        Application.targetFrameRate = 0;
    }

    private void OnEnable()
    {
        //_playerController.OnAttach += OnAttach;
        //LengthIncreaser.OnIncreased += Extend;
        GameManager.OnCompleted += RollUp;
    }

    private void OnDisable()
    {
        GameManager.OnCompleted -= RollUp;
        //_playerController.OnAttach -= OnAttach;
        //LengthIncreaser.OnIncreased -= Extend;
    }

    private void OnDestroy()
    {
        if (_blowableObject != null)
            _blowableObject.OnBlowOut -= OnDetach;

        if (_playerController != null)
            _playerController.OnAttach -= OnAttach;
    }

    private void FixedUpdate()
    {
        ChangeLength();
    }

    private void ChangeLength()
    {
        if (_ropeGenerated == true && GameManager.isPlaying == true)
        {
            float strain = _rope.CalculateLength() / _rope.restLength;
            //Debug.Log("strin: " + strain + " length: " + _curLength);
            //if(Time.frameCount % 10 == 0)
            {
                float newLength = _curLength;

                if (strain > 1.02f)
                    newLength = Mathf.Clamp(_curLength + _increaseLength, _minLength, _maxLength);
                else if (strain < .98f)
                    newLength = Mathf.Clamp(_curLength - _decreaseLength, _minLength, _maxLength);

                if (_curLength != newLength)
                {
                    if (newLength > _curLength)
                    {
                        _changeStatus = ChangeStatus.Increase;
                    }
                    else
                    {
                        if (_changeStatus == ChangeStatus.Increase)
                            return;
                        else
                            _changeStatus = ChangeStatus.Decrease;
                    }

                    //Debug.Log((_curLength < newLength ? "INCREASE" : "DECREASE") + " " + newLength);

                    if (_changeLengthCor != null)
                        _changeLengthCor.Stop();

                    _changeLengthCor = this.LerpCoroutine(
                        time: _changeLengthTime,
                        from: _curLength,
                        to: newLength,
                        action: a => _cursor.ChangeLength(a),
                        onEnd: () => _changeStatus = ChangeStatus.Idle
                    );

                    _curLength = newLength;
                }
            }
        }
    }

    private void RollUp()
    {
        if (_changeLengthCor != null)
            _changeLengthCor.Stop();

        _changeLengthCor = this.LerpCoroutine(
            time: _changeLengthTime,
            from: _curLength,
            to: 0f,
            action: a => _cursor.ChangeLength(a),
            onEnd: () => _changeStatus = ChangeStatus.Idle
        );
    }

    public void Init(Transform startPoint, ObiCollider startObiCollider, CharacterController playerController)
    {
        if(_playerController != null)
            _playerController.OnAttach -= OnAttach;

        _playerController = playerController;
        _playerController.OnAttach += OnAttach;
        _startPoint = startPoint;
        _startObiCollider = startObiCollider;

        StartCoroutine(GenerateBlueprint(_hosePoint.transform));
    }

    private void OnAttach(IBlowable blowableObject)
    {
        _blowableObject = blowableObject;
        _blowableObject.OnBlowOut += OnDetach;
        _hosePump.Activated = true;
        _rope.surfaceCollisions = false;
    }

    private void OnDetach()
    {
        _blowableObject.OnBlowOut -= OnDetach;
        _blowableObject = null;
        _hosePump.Activated = false;
        _rope.surfaceCollisions = true;
    }

    private IEnumerator GenerateBlueprint(Transform target)
    {
        _blueprint = ScriptableObject.CreateInstance<ObiRopeBlueprint>();
        _blueprint.thickness = .1f;
        _blueprint.resolution = _resolution;
        _blueprint.pooledParticles = _particlePoolSize;

        // calculate rope origin in solver space:
        Vector3 startLocalPos = _rope.transform.InverseTransformPoint(_startPoint.position);
        Vector3 endLocalPos = _rope.transform.InverseTransformPoint(target.position);

        // update direction and distance to hook point:
        Vector3 direction = target.position - startLocalPos;
        float distance = direction.magnitude;
        direction.Normalize();

        if (distance > _maxLength)
            yield break;
        else
            yield return null;

        // Clear pin constraints:
        var pinConstraints = _rope.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
        pinConstraints.Clear();

        // Procedurally generate the rope path (just a short segment, as we will extend it over time):
        int filter = ObiUtils.MakeFilter(ObiUtils.CollideWithEverything, 0);
        _blueprint.path.Clear();
        _blueprint.path.AddControlPoint(startLocalPos, Vector3.zero, Vector3.zero, Vector3.up, _particleMass, 0.1f, 1, filter, Color.white, "start");
        _blueprint.path.AddControlPoint(startLocalPos + direction * distance * .1f, Vector3.zero, Vector3.zero, Vector3.up, _particleMass, 0.1f, 1, filter, Color.white, "start2");
        _blueprint.path.AddControlPoint(startLocalPos + direction * distance * .9f, Vector3.zero, Vector3.zero, Vector3.up, _particleMass, 0.1f, 1, filter, Color.white, "end2");
        _blueprint.path.AddControlPoint(endLocalPos, Vector3.zero, Vector3.zero, Vector3.up, _particleMass, 0.1f, 1, filter, Color.white, "end");
        _blueprint.path.FlushEvents();

        // Generate the particle representation of the rope (wait until it has finished):
        yield return _blueprint.Generate();

        // Set the blueprint (this adds particles/constraints to the solver and starts simulating them).
        _rope.ropeBlueprint = _blueprint;

        // wait one frame:
        yield return null;

        _rope.GetComponent<MeshRenderer>().enabled = true;

        // set masses to zero, as we're going to override positions while we extend the rope:
        for (int i = 0; i < _rope.activeParticleCount; ++i)
            _solver.invMasses[_rope.solverIndices[i]] = 0;

        float length = 0;

        for (int i = 0; i < _rope.elements.Count; ++i)
        {
            _solver.positions[_rope.elements[i].particle1] = startLocalPos + direction * length;
            _solver.positions[_rope.elements[i].particle2] = startLocalPos + direction * (length + _rope.elements[i].restLength);
            length += _rope.elements[i].restLength;
        }

        // restore masses so that the simulation takes over now that the rope is in place:
        for (int i = 0; i < _rope.activeParticleCount; ++i)
            _solver.invMasses[_rope.solverIndices[i]] = 10; // 1/0.1 = 10

        // Pin both ends of the rope (this enables two-way interaction between character and rope):
        //var batch = new ObiPinConstraintsBatch();
        //batch.AddConstraint(_rope.elements[0].particle1, _startObiCollider, transform.localPosition, Quaternion.identity, 0, 0, float.PositiveInfinity);
        //batch.AddConstraint(_rope.elements[_rope.elements.Count - 1].particle2, target.GetComponent<ObiColliderBase>(),
        //                                                  Vector3.zero, Quaternion.identity, 0, 0, float.PositiveInfinity);
        //batch.activeConstraintCount = 2;
        //pinConstraints.AddBatch(batch);

        _rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
        _minLength = _curLength = distance;
        _cursor.ChangeLength(_curLength);

        _particleAttachments = new ObiParticleAttachment[4];

        for (int i = 0; i < 4; i++)
        {
            _particleAttachments[i] = gameObject.AddComponent<ObiParticleAttachment>();
            _particleAttachments[i].attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;
            _particleAttachments[i].target = i < 2 ? _startPoint : _hosePoint.transform;
        }

        _particleAttachments[0].particleGroup = _blueprint.groups.Find(x => x.name == "start");
        _particleAttachments[1].particleGroup = _blueprint.groups.Find(x => x.name == "start2");
        _particleAttachments[2].particleGroup = _blueprint.groups.Find(x => x.name == "end2");
        _particleAttachments[3].particleGroup = _blueprint.groups.Find(x => x.name == "end");
        _particleAttachments[1].compliance = 1f;

        _hosePoint.Init(_playerController);

        yield return new WaitForSeconds(1f);
        _ropeGenerated = true;
    }
}
