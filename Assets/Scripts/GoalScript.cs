using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class GoalScript : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);

        if(other.gameObject.tag == "Player")
        {
            EventManager.Instance.Raise(new GoalHasBeenReachedEvent());
            Destroy(gameObject);
        }
    }
}
