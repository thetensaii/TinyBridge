using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Path : SimpleGameStateObserver
{

    public GameObject frontNode;
    public GameObject backNode;


    bool _isThroughPlatform = false;

    void Start()
    {
        gameObject.SetActive(false);

        if (frontNode != null && backNode != null)
        {
            UpdateCylinder();
            UpdateJoints();
        }
    }

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

    public void SetNodes(GameObject front, GameObject back)
    {
        frontNode = front;
        backNode = back;
        UpdateCylinder();
    }

    void StructureFinished(StructureFinishedEvent e)
    {
        gameObject.GetComponent<Collider>().isTrigger = false;
    }

    public void UpdateJoints()
    {
        HingeJoint[] hingeJoints = GetComponents<HingeJoint>();
        foreach (HingeJoint hj in hingeJoints)
        {
            Destroy(hj);
        }

        HingeJoint frontJoint = gameObject.AddComponent<HingeJoint>();
        frontJoint.connectedBody = frontNode.GetComponent<Rigidbody>();
        frontJoint.axis = Vector3.forward;

        frontJoint.autoConfigureConnectedAnchor = false;
        frontJoint.anchor = Vector3.up;
        frontJoint.connectedAnchor = Vector3.zero;


        HingeJoint backJoint = gameObject.AddComponent<HingeJoint>();
        backJoint.connectedBody = backNode.GetComponent<Rigidbody>();
        backJoint.axis = Vector3.forward;

        backJoint.autoConfigureConnectedAnchor = false;
        backJoint.anchor = Vector3.down;
        backJoint.connectedAnchor = Vector3.zero;

        SetColor(Color.blue);
    }
    public void UpdateCylinder()
    {
        float newLength = Vector3.Distance(frontNode.transform.position, backNode.transform.position) / 2;
        transform.localScale = new Vector3(transform.localScale.x, newLength, transform.localScale.z);
        transform.position = backNode.transform.position - (backNode.transform.position - frontNode.transform.position) / 2;
        transform.up = frontNode.transform.position - backNode.transform.position;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            _isThroughPlatform = true;
            SetColor(Color.red);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            SetColor(Color.white);
            _isThroughPlatform = false;
        }
    }

    public bool IsThroughPlatform()
    {
        return _isThroughPlatform;
    }

    void SetColor(Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }

}
