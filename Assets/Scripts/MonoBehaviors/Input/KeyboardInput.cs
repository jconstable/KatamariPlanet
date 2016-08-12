using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Pushable))]
public class KeyboardInput : MonoBehaviour
{
    private Pushable _pushable;
    private FollowCamera _followCamera;

    // Use this for initialization
    void Start()
    {
        _pushable = gameObject.GetComponent<Pushable>();
        _followCamera = GameObject.FindObjectOfType<FollowCamera>();

        Debug.Assert(_followCamera != null, "KeyboardInput: No follow camera exists in the scene");
    }

    // Update is called once per frame
    void Update()
    {
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


        if (Input.GetKey(KeyCode.R))
        {
            _pushable.Reset();
        }
    }
}
