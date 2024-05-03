using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BulletScript : NetworkBehaviour
{
    public float speed = 20f;
    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        //GetComponent<Rigidbody>().velocity = this.transform.forward * speed;
        //GetComponent<Rigidbody>().AddForce(this.transform.forward); 
    }
}
