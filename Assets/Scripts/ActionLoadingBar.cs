using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionLoadingBar : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _fillImage;
    [SerializeField] private float _fadeTime = .1f;

    private Camera _camera;

    private CoroutineItem _loadingCor;

    public void StartLoading(float duration, Action onComplete)
    {
        if (_loadingCor != null)
            _loadingCor.Stop();

        _fillImage.fillAmount = 0f;

        _loadingCor = this.LerpCoroutine(
            time: duration,
            from: 0f,
            to: 1f,
            action: a =>
            {
                _canvasGroup.alpha = a / _fadeTime;
                _fillImage.fillAmount = a;
            },
            onEnd: () =>
            {
                onComplete();
                Hide();
            }
        );
    }

    public void CancelLoading()
    {
        Hide();
    }

    private void Hide()
    {
        if (_loadingCor != null)
            _loadingCor.Stop();

        float startValue = _canvasGroup.alpha;

        _loadingCor = this.LerpCoroutine(
            time: _fadeTime,
            from: startValue,
            to: 0f,
            action: a => _canvasGroup.alpha = a,
            onEnd: () => _fillImage.fillAmount = 0f
        );
    }

    private void Start()
    {
        _camera = Camera.main;
        _fillImage.fillAmount = 0f;
        _canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(-_camera.transform.forward, _camera.transform.up);
    }
}