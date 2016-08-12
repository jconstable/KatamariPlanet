using UnityEngine;
using System.Collections;

public class FakeGravity : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        transform.up = transform.position.normalized;
        GameObject planet = GameObject.FindGameObjectWithTag(Tags.PlanetTag);
        if (planet != null)
        {
            PlanetConstants constants = planet.GetComponent<PlanetConstants>();

            float halfBounds = 0f;
            Renderer r = gameObject.GetComponent<Renderer>();
            if (r != null)
            {
                halfBounds = (r.bounds.size.y / 2f);
            }

            float heightCorrection = 0f;
            if (constants != null)
            {
                heightCorrection = constants.ObjectHeightCorrection;
            }

            transform.position = transform.up * ((planet.transform.localScale.y / 2f) + halfBounds + heightCorrection);
        }

    }
}
