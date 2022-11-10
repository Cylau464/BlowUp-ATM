using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class ATM : MonoBehaviour, IBlowable
{
    [Header("Blowing")]
    [SerializeField] private Transform _attachPoint;
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private ActionLoadingBar _blowPrepareBar;
    [SerializeField] private ParticleSystem _blowParticle;
    [Space(2f)]
    [SerializeField] private float _blowTime = 2f;
    [SerializeField] private float _blowExlosionForce = 2f;
    [SerializeField] private float _blowExplosionRadius = 2f;
    [Space(2f)]
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _flyingForce = 1f;
    [SerializeField] private float _flyingTorque = .5f;
    [Header("Money")]
    [SerializeField] private Money _moneyPrefab;
    [SerializeField] private Transform _moneySpawnPoint;
    [SerializeField] private float _moneyRadiusSpawn = 2f;
    [SerializeField] private int _moneyCount = 20;
    public int MoneyCount => _moneyCount * _moneyPrefab.Price;

    private List<Money> _money = new List<Money>();

    private Vector3 _torqueDirection;

    public BlowState State { get; private set; }

    public Action OnStartBlow { get; set; }
    public Action OnBlowOut { get; set; }

    private void Start()
    {
        for(int i = 0; i < _moneyCount; i++)
        {
            Money money = Instantiate(_moneyPrefab, Random.insideUnitSphere * _moneyRadiusSpawn + _moneySpawnPoint.position, Random.rotation);
            money.transform.parent = _moneySpawnPoint;
            _money.Add(money);
        }

        _moneySpawnPoint.gameObject.SetActive(false);
        State = BlowState.NotBlow;
    }

    private void FixedUpdate()
    {
        if(State == BlowState.Blowing)
        {
            _rigidBody.AddTorque(_torqueDirection * _flyingTorque, ForceMode.Acceleration);
            _rigidBody.AddForce(Vector3.up * _flyingForce, ForceMode.Acceleration);
        }
    }

    public void PrepareBlow(float duration, Action onComplete)
    {
        _blowPrepareBar.StartLoading(duration, onComplete);
    }

    public void CancelPrepare()
    {
        _blowPrepareBar.CancelLoading();
    }

    public void StartBlow()
    {
        State = BlowState.Blowing;
        _torqueDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        StopAllCoroutines();
        
        this.LerpCoroutine(
            time: _blowTime,
            from: 0f,
            to: 100f,
            action: a => _renderer.SetBlendShapeWeight(0, a),
            onEnd: () => BlowOut()
        );

        OnStartBlow?.Invoke();

        Dictionary<string, object> eventInfo = new Dictionary<string, object>()
        {
            { "level_id", GameManager.Instance.levelsData.idLevel }
        };
        apps.EventsLogger.CustomEvent("atm_blowed", eventInfo);
    }

    public void BlowOut()
    {
        State = BlowState.Blowed;
        OnBlowOut?.Invoke();
        SpawnMoney();
        _blowParticle.Play();
        _blowParticle.transform.parent = null;
        Destroy(gameObject);
    }

    public Transform GetAttachPoint()
    {
        return _attachPoint;
    }

    private void SpawnMoney()
    {
        _moneySpawnPoint.parent = null;
        _moneySpawnPoint.rotation = Quaternion.identity;
        _moneySpawnPoint.gameObject.SetActive(true);

        foreach (Money money in _money)
            money.AddExplosionForce(transform.position, _blowExlosionForce, _blowExplosionRadius);
    }
}