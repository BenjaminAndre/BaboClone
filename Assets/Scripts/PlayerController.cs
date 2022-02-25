using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float acceleration = 20;//unit is m/s²
    [SerializeField] private float decelerationBonus = 2F;
    [SerializeField] private string playerName = "Junior";

    private Team teamMembership;

    [SerializeField] private float maxHealth = 100;
    private float health;

    private Vector3 steeringDirection;

    public override void OnNetworkSpawn()
    {
        health = maxHealth;
    }

    [ServerRpc]
    void SetSteeringDirectionServerRpc(Vector3 direction) {
        this.steeringDirection = direction.normalized;
    }


    [ServerRpc]
    public void SufferDamageServerRpc(float damageTaken) {
        this.health -= damageTaken;
        Debug.Log("Aouch, I, " + playerName + ", has only " + health + " hp left.");
        if(health <= 0) {
            Destroy(this.gameObject);
        }
    }



    void Update()
    {
        if(IsOwner){
            float vf = Input.GetAxis("Vertical");
            float hf = Input.GetAxis("Horizontal");
            Vector3 moveDirection = new Vector3(-vf, 0, hf).normalized;
            SetSteeringDirectionServerRpc(moveDirection);
        }
    }

    void FixedUpdate() {
        if(IsServer){
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            
            float forwardComponent = Vector3.Dot(rb.velocity, steeringDirection);

            if(forwardComponent < 0) {
                Debug.Log(forwardComponent);
                Vector3 acc = rb.velocity.normalized * forwardComponent * decelerationBonus;
                Debug.Log("Velocity : " + rb.velocity.magnitude + " Brake vel : " + acc.magnitude);
                rb.AddForce(acc, ForceMode.Acceleration);
            }

            
            Vector3 desiredAcceleration = steeringDirection * acceleration;
            rb.AddForce(desiredAcceleration, ForceMode.Acceleration);
        }
    }

    public Team GetTeam() {
        return ConnectionsManager.GetTeam(this);
    }
}