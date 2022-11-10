using main.level;
using System;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;


public class LeaveArea : MonoBehaviour
{
    [SerializeField] private float _prepareTime = 2f;
    [SerializeField] private ActionLoadingBar _prepareBar;
    [SerializeField] private Transform _leaveTarget;
    [SerializeField] private Van _van;
    [SerializeField] private PlayableDirector _leaveCutscene;
    [Space]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _fadeTime = .2f;
    [Space]
    [SerializeField] private float _iridescentTime = 1f;
    [SerializeField] private Color _enabledColor = Color.green;
    private Color _baseColor;

    private bool _enabled;
    private CharacterController _playerController;

    public Action OnStartLeave { get; set; }
    public Action OnCancelLeave { get; set; }

    private void Start()
    {
        LevelsManager.currentLevel.OnCollectMoney += OnCollectMoney;
        _baseColor = _sprite.color;

        if (GameManager.isStarted == false)
            HideGraphics(true);
    }

    private void Update()
    {
        if(_enabled == true)
        {
            _sprite.color = Color.Lerp(_baseColor, _enabledColor, Mathf.PingPong(Time.time / _iridescentTime, 1f));
            _text.color = Color.Lerp(_baseColor, _enabledColor, Mathf.PingPong(Time.time / _iridescentTime, 1f));
        }
    }

    private void OnDestroy()
    {
        if(LevelsManager.currentLevel != null)
            LevelsManager.currentLevel.OnCollectMoney -= OnCollectMoney;

        if(_playerController != null)
            _playerController.OnReachedLeaveTarget -= StartCutscene;
    }

    private void OnCollectMoney(int count)
    {
        if (_enabled == false && count >= LevelsManager.currentLevel.MinMoney)
        {
            _enabled = true;
            ShowGraphics();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_enabled == true && other.TryGetComponent(out CharacterController cc) == true)
        {
            _prepareBar.StartLoading(_prepareTime, Leave);
            _playerController = cc;
            OnStartLeave?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_enabled == true && other.TryGetComponent(out CharacterController cc) == true)
        {
            _prepareBar.CancelLoading();
            _playerController = null;
            OnCancelLeave?.Invoke();
        }
    }

    private void HideGraphics(bool instantly)
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

    private void ShowGraphics()
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

    private void Leave()
    {
        HideGraphics(false);
        _playerController.Leave(_leaveTarget);
        _enabled = false;
        _sprite.color = _baseColor;
        _text.color = _baseColor;
        _playerController.OnReachedLeaveTarget += StartCutscene;

        GameManager.Instance.MakeCompleted();
    }

    private void StartCutscene()
    {
        _leaveCutscene.Play();
        _playerController.OnReachedLeaveTarget -= StartCutscene;
    }
}
