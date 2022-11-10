using UnityEngine;

public class TargetPointer : MonoBehaviour
{
    [SerializeField] private float _localHeight = 5f;
    [SerializeField] private float _moveInterval = 1f;
    [SerializeField] private float _moveAmplitude = .5f;
    [SerializeField] private float _scaleInterval = 1f;
    [SerializeField] private float _scaleAmplitude = .1f;
    [SerializeField] private bool _hideOnTrigger = true;

    private Vector3 _startPos;
    private Vector3 _startScale;

    private bool _active = true;

    private void Awake()
    {
        _startPos = transform.localPosition;
        _startScale = transform.localScale;
    }

    private void Update()
    {
        if (_active == true)
        {
            transform.localScale = _startScale + Vector3.one * Mathf.PingPong(Time.time / _scaleInterval, _scaleAmplitude);
        }

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.up * (Mathf.PingPong(Time.time / _moveInterval, _moveAmplitude) + _localHeight)
            + new Vector3(transform.parent.position.x, 0f, transform.parent.position.z);
    }

    public void Activate()
    {
        gameObject.SetActive(true);

        StopAllCoroutines();
        this.LerpCoroutine(
            time: .2f,
            from: 0f,
            to: _startScale.x,
            action: a => transform.localScale = Vector3.one * a,
            onEnd: () => _active = true
        );
    }

    public void Deactivate(bool instantly)
    {
        if (gameObject.activeInHierarchy == false) return;

        StopAllCoroutines();

        if (instantly == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
            this.LerpCoroutine(
                time: .2f,
                from: transform.localScale.x,
                to: 0f,
                action: a => transform.localScale = Vector3.one * a,
                onEnd: () => 
                {
                    _active = false;
                    gameObject.SetActive(false);
                }
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hideOnTrigger == true && other.TryGetComponent(out CharacterController cc) == true)
        {
            Deactivate(false);
        }
    }
}
