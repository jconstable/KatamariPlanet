using UnityEngine;
using System.Collections;
using UnityEditor;

public class KatamariUtils : MonoBehaviour {

	[MenuItem("Katamari/Add Mass Components")]
    public static void AddMassComponents()
    {
        Collider[] allColliders = GameObject.FindObjectsOfType<Collider>();
        foreach( Collider c in allColliders)
        {
            c.gameObject.AddComponent<KatamariMass>();
        }
    }
}
