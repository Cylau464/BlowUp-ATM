using System.Collections;
using UnityEngine;

public class Hat : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Collider _collider;

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
        _collider.enabled = false;
        _rigidBody.isKinematic = true;
    }

    private void Detach()
    {
        transform.parent = null;
        _collider.enabled = true;
        _rigidBody.isKinematic = false;
    }
}