using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BounceOnCollide : MonoBehaviour
{
    public float Bounciness = 1f;

    private Rigidbody _rigidBody;
    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 impulse = collision.impulse;
        impulse *= Bounciness;

        _rigidBody.AddForce(impulse, ForceMode.Impulse);
    }
}
