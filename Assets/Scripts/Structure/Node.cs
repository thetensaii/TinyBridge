using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SDD.Events;


public class Node : SimpleGameStateObserver
{
    //Prefabs
    public GameObject nodePrefab;
    public GameObject pathPrefab;

    // Path Lengths Limits
    public float MaxPathLength = 3f;
    public float MinPathLength = 0.25f;

    // Nouveau point qui sera créer lorsque l'on clique sur ce Node
    private GameObject newNode;

    // Les deux paths qui seront reliés au nouveau Node 
    private GameObject fromThisNodePath; // Path lié à ce Node
    private GameObject fromOtherNodePath; // Path lié à l'autre Node le plus proche

    float fromThisNodePathLength;
    float fromOtherNodePathLength;

    // Offset de la souris
    private Vector3 offset;

    // dfd
    private bool canCreateNewNode = true;

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();

        // HUDManager
        EventManager.Instance.AddListener<StructureFinishedEvent>(StructureFinished);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();

        // HUDManager
        EventManager.Instance.RemoveListener<StructureFinishedEvent>(StructureFinished);
    }

    void StructureFinished(StructureFinishedEvent e)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        canCreateNewNode = false;
    }

    private void OnMouseDown()
    {
        if (!canCreateNewNode) return;

        // Création des nouveaux Node at Paths
        newNode = Instantiate(nodePrefab, transform.parent);
        fromThisNodePath = Instantiate(pathPrefab, transform.parent);
        fromOtherNodePath = Instantiate(pathPrefab, transform.parent);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    }

    private void OnMouseDrag()
    {
        if (!canCreateNewNode) return;
        if (newNode == null) return;

        // Mise à jour nouvelle position du nouveau Node
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        newNode.transform.position = curPosition;

        // Mise à jour des paths en fonction de la position du Node
        fromThisNodePathLength = Vector3.Distance(curPosition, transform.position);

        if ( fromThisNodePathLength > MaxPathLength || fromThisNodePathLength < MinPathLength) {
            if (fromThisNodePath != null) {
                Destroy(fromThisNodePath);
                fromThisNodePath = null;
            }
        } else {
            if (fromThisNodePath == null) {
                fromThisNodePath = Instantiate(pathPrefab, transform.parent);
            } else {
                fromThisNodePath.GetComponent<Path>().SetNodes(newNode, gameObject);
            }
        }

        GameObject closestNode = GetClosestNode();
        fromOtherNodePathLength = Vector3.Distance(curPosition, closestNode.transform.position);

        if (fromOtherNodePathLength > MaxPathLength || fromOtherNodePathLength < MinPathLength) {
            if (fromOtherNodePath != null) {
                Destroy(fromOtherNodePath);
                fromOtherNodePath = null;
            }
        } else {
            if (fromOtherNodePath == null) {
                fromOtherNodePath = Instantiate(pathPrefab, transform.parent);
            } else {
                fromOtherNodePath.GetComponent<Path>().SetNodes(newNode, closestNode);
            }
        }
    }

    private void OnMouseUp()
    {
        if (!canCreateNewNode) return;

        // Création des HingeJoints
        if (newNode != null && fromOtherNodePath != null && fromThisNodePath != null &&
            !fromThisNodePath.GetComponent<Path>().IsThroughPlatform() && !fromOtherNodePath.GetComponent<Path>().IsThroughPlatform()) { 
            fromThisNodePath.GetComponent<Path>().UpdateCylinder();
            fromOtherNodePath.GetComponent<Path>().UpdateCylinder();

            fromThisNodePath.GetComponent<Path>().UpdateJoints();
            fromOtherNodePath.GetComponent<Path>().UpdateJoints();

            EventManager.Instance.Raise(new NodeHasBeenCreatedEvent());
        } else {
            Destroy(fromThisNodePath);
            Destroy(fromOtherNodePath);
            Destroy(newNode);

            fromThisNodePath = null;
            fromOtherNodePath = null;
            newNode = null;
        }
    }

    
    private GameObject GetClosestNode()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        int index = -1;
        float minValue = 9999;

        float[] distances = new float[nodes.Length];
        for (int i = 0; i < nodes.Length; ++i) {
            if(nodes[i] == gameObject || nodes[i] == newNode) {
                distances[i] = 9999999;
            } else {
                distances[i] = Vector3.Distance(newNode.transform.position, nodes[i].transform.position);
            }
        }
        
        for(int i = 0; i < nodes.Length; i++) {
            if (distances[i] < minValue) {
                index = i;
                minValue = distances[i];
            }
        }

        return nodes[index];
    }
}
