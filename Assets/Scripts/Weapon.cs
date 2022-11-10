using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _bulletPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private ParticleSystem _shotParticle;
    [SerializeField] private float _shockDuration = 3f;

    [Space]
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Collider _collider;
    [SerializeField] private EnemyController _controller;

    private Stack<Bullet> _bulletPool;

    private void OnEnable()
    {
        _controller.OnStartBlow += Detach;
    }

    private void OnDisable()
    {
        _controller.OnStartBlow -= Detach;
    }

    private void Start()
    {
        _rigidBody.isKinematic = true;
        _collider.enabled = false;
    }

    private void OnDestroy()
    {
        foreach (Bullet bullet in _bulletPool)
        {
            if(bullet != null)
                bullet.OnReturn -= ReturnBulletToPool;
        }
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.W))
        //{
        //    transform.parent = null;
        //    Debug.Break();
        //}
    }

    public void Init(Collider ignoredCollider)
    {
        _bulletPool = new Stack<Bullet>();

        for (int i = 0; i < 2; i++)
        {
            Bullet bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
            bullet.transform.SetParent(transform, true);
            bullet.Init(ignoredCollider);
            bullet.OnReturn += ReturnBulletToPool;
            _bulletPool.Push(bullet);
        }
    }

    public void Shot(Vector3 direction)
    {
        Bullet bullet = _bulletPool.Pop();
        bullet.Launch(_bulletPoint.position, direction, _shockDuration);
        _shotParticle.Play();
    }

    public void ReturnBulletToPool(Bullet bullet)
    {
        bullet.transform.SetParent(transform, true);
        bullet.transform.localPosition = Vector3.zero;
        bullet.gameObject.SetActive(false);
        _bulletPool.Push(bullet);
    }

    private void Detach()
    {
        transform.parent = null;
        _rigidBody.isKinematic = false;
        _collider.enabled = true;
    }
}