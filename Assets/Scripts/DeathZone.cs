using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class DeathZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") EventManager.Instance.Raise(new PlayerHasHitDeathZoneEvent());
    }

}
