using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PlayerScript : SimpleGameStateObserver
{

    public float m_Speed;
    public float m_jumpForce;
    private Rigidbody m_Rigidbody;
    private Transform m_Transform;

    private bool isOnFloor = true;

    private bool canMove = false;

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
    protected override void Awake()
    {
        base.Awake();

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Transform = GetComponent<Transform>();
    }

    void StructureFinished(StructureFinishedEvent e)
    {
        canMove = true;
    }


    void FixedUpdate()
    {
        //Debug.Log(isOnFloor);
        if (!canMove) return;

        float horizontalMovement = Input.GetAxis("Horizontal");
        bool jump = Input.GetAxis("Jump") > 0 || Input.GetKeyDown(KeyCode.Space);
        Debug.Log(jump);

        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Transform.right * m_Speed * horizontalMovement * Time.fixedDeltaTime);
        

        if (jump && isOnFloor)
        {
            Vector3 jumpForce = Vector3.up * m_jumpForce;
            m_Rigidbody.AddForce(jumpForce, ForceMode.Impulse);
        }

        if (isOnFloor)
        {
            m_Rigidbody.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isOnFloor = true;
            Vector3 colLocalPt = m_Transform.InverseTransformPoint(collision.contacts[0].point);
            if (colLocalPt.magnitude < 0.25f) isOnFloor = true;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9) isOnFloor = false;
    }
}
