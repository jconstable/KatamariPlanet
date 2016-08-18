using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BounceOnCollide : MonoBehaviour
{
    public float Bounciness = 1f;
    public string ThudSoundName;
    public float VerticalDotMaxForThud = 0.01f;

    private Rigidbody _rigidBody;
    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 impulse = collision.impulse;
        if (impulse.sqrMagnitude > 0f)
        {
            impulse *= -Bounciness;

            _rigidBody.AddForce(impulse, ForceMode.Impulse);

            float dot = Vector3.Dot(_rigidBody.velocity.normalized, _rigidBody.transform.position.normalized);
            if ( dot < VerticalDotMaxForThud )
            {
                if (!string.IsNullOrEmpty(ThudSoundName))
                {
                    KatamariAppProxy.instance.GetSoundManager().PlayCustomSound(ThudSoundName);
                }
            }
        }
    }
}
