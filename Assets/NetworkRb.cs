using Unity.Netcode;
using UnityEngine;

public class NetworkRigidbody : NetworkBehaviour
{
    private Rigidbody rb;

    private NetworkVariable <Vector3> netPosition = new NetworkVariable<Vector3>();
    private NetworkVariable <Quaternion> netRotation = new NetworkVariable<Quaternion>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Start()
    {
       
        if (IsServer)
        {
            rb.isKinematic = false;
        }
        else
        {
            rb.isKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            
            netPosition.Value = rb.position;
            netRotation.Value = rb.rotation;
        }
        else
        {
            
            rb.position = netPosition.Value;
            rb.rotation = netRotation.Value;
        }
    }
}