using UnityEngine;

public class AgroSign : MonoBehaviour
{
    [SerializeField] private float _showDuration = 1f;
    [SerializeField] private float _fadeTime = .1f;
    [SerializeField] private float _scaleIncrease = .2f;
    [SerializeField] private Renderer[] _renderers;

    private MaterialPropertyBlock _propertyBlock;
    private Color _color;
    private Vector3 _startScale;

    private const string colorPropertyName = "_BaseColor";

    private void Start()
    {
        _propertyBlock = new MaterialPropertyBlock();
        _color = _renderers[0].sharedMaterial.GetColor(colorPropertyName);
        _startScale = transform.localScale;

        foreach (Renderer renderer in _renderers)
            renderer.GetPropertyBlock(_propertyBlock);

        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Color startColor = _color;
        startColor.a = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = _startScale + Vector3.one * _scaleIncrease;

        this.LerpCoroutine(
            time: _showDuration,
            from: 0f,
            to: 1f,
            action: a =>
            {
                transform.localScale = Vector3.Lerp(startScale, targetScale, a);
                _propertyBlock.SetColor(colorPropertyName, Color.Lerp(startColor, _color, a / _fadeTime));

                foreach (Renderer renderer in _renderers)
                    renderer.SetPropertyBlock(_propertyBlock);
            },
            settings: new CoroutineTemplate.Settings(pingPong: true),
            onEnd: () => gameObject.SetActive(false)
        );

    }
}