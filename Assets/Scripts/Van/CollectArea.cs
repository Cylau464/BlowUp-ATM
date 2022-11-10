using System;
using UnityEngine;
using TMPro;

public class CollectArea : MonoBehaviour
{
    [SerializeField] private float _collectMinSpeed = 5f; // count of money which collected per second
    [SerializeField] private float _collectMaxSpeed = 25f;
    [SerializeField] private float _acceleration = 1f;
    [SerializeField] private MoneyCollector _moneyCollector;
    [Space]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _fadeTime = .2f;
    [Space]
    [SerializeField] private float _iridescentTime = 1f;
    [SerializeField] private Color _enabledColor = Color.green;
    private Color _baseColor;

    private bool _enabled;

    private MoneyHolder _moneyHolder;
    private float _curCollectProgress;
    private float _curCollectSpeed;

    public Action OnStartCollect;
    public Action OnStopCollect;

    private void OnEnable()
    {
        GameManager.OnCompleted += Hide;
        MoneyHolder.OnEmpty += Disable;
        MoneyHolder.OnFill += Enabled;
    }

    private void OnDisable()
    {
        GameManager.OnCompleted -= Hide;
        MoneyHolder.OnEmpty -= Disable;
        MoneyHolder.OnFill -= Enabled;
    }

    private void Enabled()
    {
        _enabled = true;
    }

    private void Disable()
    {
        _enabled = false;
        _sprite.color = _baseColor;
        _text.color = _baseColor;
    }


    private void Start()
    {
        _curCollectSpeed = _collectMinSpeed;
        _baseColor = _sprite.color;

        if (GameManager.isStarted == false)
        {
            Hide(true);
            GameManager.OnLevelStarted += Show;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnLevelStarted -= Show;
    }

    private void Update()
    {
        if(_moneyHolder != null)
        {
            _curCollectSpeed = Mathf.Clamp(
                _curCollectSpeed + _acceleration * Time.deltaTime,
                _collectMinSpeed,
                _collectMaxSpeed
            );
            _curCollectProgress += Time.deltaTime * _curCollectSpeed;

            if(_curCollectProgress >= 1f)
            {
                _curCollectProgress %= 1;
                Money money = _moneyHolder.GetMoney();

                if(money != null)
                    _moneyCollector.Collect(money);
            }
        }

        if (_enabled == true)
        {
            _sprite.color = Color.Lerp(_baseColor, _enabledColor, Mathf.PingPong(Time.time / _iridescentTime, 1f));
            _text.color = Color.Lerp(_baseColor, _enabledColor, Mathf.PingPong(Time.time / _iridescentTime, 1f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_enabled == true && _moneyHolder == null && other.TryGetComponent(out CharacterController cc) == true)
        {
            _moneyHolder = cc.MoneyHolder;
            _curCollectProgress = 0f;
            OnStartCollect?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_moneyHolder != null && other.TryGetComponent(out CharacterController cc) == true)
        {
            _moneyHolder = null;
            _curCollectSpeed = _collectMinSpeed;
            OnStopCollect?.Invoke();
        }
    }

    private void Hide()
    {
        Hide(false);
    }

    private void Hide(bool instantly)
    {
        Color startSpriteColor = _sprite.color;
        Color targetSpriteColor = startSpriteColor;
        targetSpriteColor.a = 0f;
        Color startTextColor = _text.color;
        Color targetTextColor = startTextColor;
        targetTextColor.a = 0f;

        this.LerpCoroutine(
            time: instantly == true ? 0f : _fadeTime,
            from: 0f,
            to: 1f,
            action: a =>
            {
                _sprite.color = Color.Lerp(startSpriteColor, targetSpriteColor, a);
                _text.color = Color.Lerp(startTextColor, targetTextColor, a);
            }
            //onEnd: () => gameObject.SetActive(false)
        );
    }

    private void Show()
    {
        Color startSpriteColor = _sprite.color;
        Color targetSpriteColor = startSpriteColor;
        targetSpriteColor.a = 1f;
        Color startTextColor = _text.color;
        Color targetTextColor = startTextColor;
        targetTextColor.a = 1f;

        this.LerpCoroutine(
            time: _fadeTime,
            from: 0f,
            to: 1f,
            action: a =>
            {
                _sprite.color = Color.Lerp(startSpriteColor, targetSpriteColor, a);
                _text.color = Color.Lerp(startTextColor, targetTextColor, a);
            }
            //onEnd: () => gameObject.SetActive(false)
        );
    }
}
