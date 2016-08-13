using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Jumpable : MonoBehaviour
{
    public float JumpForce = 2f;
    public float VerticalDotMax = 0.01f;

    private Rigidbody _rigidBody;

    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public void HandleJump()
    {
        float dot = Vector3.Dot(_rigidBody.velocity.normalized, _rigidBody.transform.position.normalized);
        Debug.Log(Mathf.Abs(dot).ToString());
        if ( Mathf.Abs(dot) < VerticalDotMax)
        {
            _rigidBody.AddForce(_rigidBody.transform.position.normalized * JumpForce, ForceMode.Impulse);
        }
    }
}