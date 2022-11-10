using Obi;
using UnityEngine;

public class HoseInitializer : MonoBehaviour
{
    [SerializeField] private Transform _hoseStartPoint;
    [SerializeField] private CharacterController _playerController;
    [SerializeField] private ObiCollider _hoseStartObiCollider;

    private void Start()
    {
        Hose.Instance.Init(_hoseStartPoint, _hoseStartObiCollider, _playerController);
    }
}