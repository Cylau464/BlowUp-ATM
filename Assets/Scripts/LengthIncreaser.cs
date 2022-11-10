using System;
using UnityEngine;

public class LengthIncreaser : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _length = 1f;

    public static Action<float> OnIncreased;

    private void OnCollisionEnter(Collision collision)
    {
        if((1 << collision.gameObject.layer & _playerLayer) != 0)
        {
            OnIncreased?.Invoke(_length);
            Destroy(gameObject);
        }
    }
}
