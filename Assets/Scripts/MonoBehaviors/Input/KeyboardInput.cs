using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Pushable))]
public class KeyboardInput : MonoBehaviour
{
    private Pushable _pushable;
    private Jumpable _jumpable;
    private FollowCamera _followCamera;

    // Use this for initialization
    void Start()
    {
        _pushable = gameObject.GetComponent<Pushable>();
        _jumpable = gameObject.GetComponent<Jumpable>();
        _followCamera = GameObject.FindObjectOfType<FollowCamera>();

        Debug.Assert(_followCamera != null, "KeyboardInput: No follow camera exists in the scene");
    }

    void UpdatePushable()
    {
        if (_pushable != null)
        {
            // Handle pushing the ball around
            Vector3 dir = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                dir += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                dir += Vector3.back;
            }
            if (Input.GetKey(KeyCode.D))
            {
                dir += Vector3.right;
            }
            if (Input.GetKey(KeyCode.A))
            {
                dir += Vector3.left;
            }
            if (dir.sqrMagnitude > 0f)
            {
                _pushable.HandlePush(dir);
            }

            // Handle resetting the ball position
            if (Input.GetKey(KeyCode.R))
            {
                _pushable.Reset();
            }
        }
    }

    void UpdateJumpable()
    {
        if( _jumpable != null )
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpable.HandleJump();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePushable();
        UpdateJumpable();
    }
}
