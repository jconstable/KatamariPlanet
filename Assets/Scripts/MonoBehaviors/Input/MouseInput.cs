using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour
{
    private FollowCamera _followCamera;

    // Use this for initialization
    void Start()
    {
        _followCamera = GameObject.FindObjectOfType<FollowCamera>();

        Debug.Assert(_followCamera != null, "KeyboardInput: No follow camera exists in the scene");
    }

    // Update is called once per frame
    void Update()
    {


    }
}
