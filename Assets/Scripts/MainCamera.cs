using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainCamera : MonoBehaviour
{
    [SerializeField] Vector3 offSet;
    private Transform localPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        try {
            var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            localPlayer = playerObject.transform;
            this.transform.position = localPlayer.position + offSet;
        } catch {
            // The local player doesn't exist yet or has been destroyed
            // TODO try to get a whole overview of the terrain by getting higher
            this.transform.position = offSet;
        }
    }
}
