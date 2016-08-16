using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour
{
    private FollowCamera _followCamera;
    private Jumpable _jumpable;

    // Use this for initialization
    void Start()
    {
        _jumpable = gameObject.GetComponent<Jumpable>();
        _followCamera = GameObject.FindObjectOfType<FollowCamera>();

        DebugUtils.Assert(_followCamera != null, "KeyboardInput: No follow camera exists in the scene");
    }

    void UpdateJumpable()
    {
        if( _jumpable != null )
        {
            if( Input.GetMouseButtonDown(0) )
            {
                _jumpable.HandleJump();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateJumpable();
    }
}
