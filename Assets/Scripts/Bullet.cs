using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Collider _collider;
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _lifeTime = 5f;

    private float _shockDuration;

    public Action<Bullet> OnReturn;

    private void Start()
    {
        _hitParticle.Stop();
    }

    public void Init(Collider ignoredCollider)
    {
        Physics.IgnoreCollision(_collider, ignoredCollider, true);
        gameObject.SetActive(false);
    }

    public void Launch(Vector3 position, Vector3 direction, float shockDuration)
    {
        Invoke(nameof(ReturnToPool), _lifeTime);
        _shockDuration = shockDuration;
        gameObject.SetActive(true);
        transform.parent = null;
        transform.position = position;
        transform.up = direction;
        _rigidBody.velocity = direction * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out CharacterController cc))
        {
            cc.Shock(_shockDuration, (cc.transform.position - transform.position).normalized);
        }

        _hitParticle.transform.position = collision.contacts[0].point;
        _hitParticle.transform.parent = null;
        _hitParticle.transform.forward = collision.contacts[0].normal;
        _hitParticle.Play();
        Invoke(nameof(ReturnParticle), _hitParticle.main.duration);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        CancelInvoke(nameof(ReturnToPool));
        OnReturn?.Invoke(this);
    }

    private void ReturnParticle()
    {
        _hitParticle.transform.parent = transform;
        _hitParticle.transform.localPosition = Vector3.zero;
    }
}