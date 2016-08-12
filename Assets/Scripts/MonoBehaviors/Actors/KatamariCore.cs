using UnityEngine;
using System.Collections;

public class KatamariCore : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Physics.gravity = transform.position.normalized * -9.8f;
    }
}
