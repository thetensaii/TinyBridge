using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureObjectScript : MonoBehaviour
{
    void Start()
    {
        // Aucune collision entre GameObjects de la structure
        GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");

        foreach (GameObject p in paths)
        {
            if (p != gameObject)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), p.GetComponent<Collider>());
            }
        }

        foreach (GameObject n in nodes)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), n.GetComponent<Collider>());
        }
    }
}
