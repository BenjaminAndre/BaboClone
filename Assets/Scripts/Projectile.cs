using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Projectile : NetworkBehaviour
{
    //private PlayerController owner;

    // The use of only initial velocity instead of a full blown synchronisation saves precious bandwidth
    [ClientRpc]
    public void InitClientRpc(Vector3 velocity) {
        //this.owner = owner;
        this.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
    }

    // Start is called before the first frame update
    void Start()
    {
        //this.GetComponent<MeshRenderer>().material = this.owner.GetTeam().GetTeamMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}   