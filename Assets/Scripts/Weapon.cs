using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Weapon : NetworkBehaviour
{

    // No weapon model : already instanciated as child

    [SerializeField]
    private GameObject projectile;

    [SerializeField, Tooltip("Delay between shots, in seconds.")]
    private float shootTimeout;


    private float lastShot;

    // Start is called before the first frame update
    void Start()
    {
        this.lastShot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner) {
            OrientWeapon();
            if (Input.GetMouseButton(0)) {
                ShootServerRpc();
            }
        }
    }


    void OrientWeapon() {
        Plane plane = new Plane(Vector3.up, 0.5f);
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);
        if(point != null) {
            this.transform.LookAt(point);
            this.transform.eulerAngles =  Vector3.Scale(this.transform.eulerAngles, Vector3.up); //remove pitch and roll
            SetWeaponDirectionServerRpc(this.transform.eulerAngles);
        }
    }

    // This overcomplex structure allows for the player to see his weapon rotate immediately. This bypass the server authority, which is ok for
    // a precisely timed visual element such as this
    [ServerRpc]
    private void SetWeaponDirectionServerRpc(Vector3 eulerAngles) {
        this.transform.eulerAngles = eulerAngles;
        SetWeaponDirectionClientRpc(eulerAngles);
    }

    [ClientRpc]
    private void SetWeaponDirectionClientRpc(Vector3 eulerAngles) {
        if(!IsOwner) {
            this.transform.eulerAngles = eulerAngles;
        }
    }



    [ServerRpc]
    private void ShootServerRpc() {
        // Preventing too early shots
        if (Time.time - this.lastShot > this.shootTimeout) {
            var proj = Instantiate(projectile, this.transform.position + this.transform.right * 0.5f + this.transform.forward * 0.5f, this.transform.rotation);
            proj.GetComponent<NetworkObject>().Spawn();
            Vector3 velocity = this.transform.forward * 40;
            proj.GetComponent<Projectile>().InitClientRpc(velocity);
            //transform.parent.GetComponent<Rigidbody>().AddForce(Vector3.Scale(proj.transform.forward * -5, new Vector3(1,0,1)));
            Destroy(proj, 2);
            this.lastShot = Time.time;
        }
    }
}
