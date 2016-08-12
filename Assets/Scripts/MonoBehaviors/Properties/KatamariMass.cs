using UnityEngine;
using System.Collections;

public class KatamariMass : MonoBehaviour
{
    private Renderer _rend;

    // Use this for initialization
    void Start()
    {
        _rend = gameObject.GetComponent<Renderer>();
    }

    public float Mass
    {
        get
        {
            float m = 0f;

            if (_rend != null)
            {
                Vector3 size = _rend.bounds.size;
                m = size.x * size.y * size.z;
            }

            return m;
        }
    }
}
