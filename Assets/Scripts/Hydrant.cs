using UnityEngine;

public class Hydrant : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private ParticleSystem _waterParticle;
    [SerializeField] private float _force = 10f;
    [SerializeField] private float _torque = 2f;

    private void OnEnable()
    {
        GameManager.OnCompleted += Activate;
    }

    private void OnDisable()
    {
        GameManager.OnCompleted -= Activate;
    }

    private void Activate()
    {
        _rigidBody.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Van van) == true)
        {
            _rigidBody.AddForce(Vector3.up * _force, ForceMode.Impulse);
            Vector3 torqueDir = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
            _rigidBody.AddTorque(torqueDir * _torque, ForceMode.Impulse);
            _waterParticle.Play();
            _waterParticle.transform.parent = null;

            this.DoAfterNextFixedFrameCoroutine(() =>
                _rigidBody.constraints = RigidbodyConstraints.None);
        }
    }
}