using Unity.Netcode;
using UnityEngine;

public class ConnectionsManager : MonoBehaviour {

    private static Team[] teams;

    void OnGUI()
    {
        teams = GetComponentsInChildren<Team>();
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
            //TeamButtons();

        }

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    static void TeamButtons()
    {
        PlayerController thePlayer = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<PlayerController>();

        if (GUILayout.Button("A")) JoinTeam(thePlayer, teams[0]);
        if (GUILayout.Button("B")) JoinTeam(thePlayer, teams[1]);
        if (GUILayout.Button("C")) JoinTeam(thePlayer, teams[2]);
    }

    static void JoinTeam(PlayerController pc, Team t) {
        //Remove it from any team
        foreach(Team team in teams) {
            team.members.Remove(pc);
        }
        t.members.Add(pc);
        pc.GetComponent<MeshRenderer>().material = t.GetTeamMaterial();
        Debug.Log("Player added to team " + t.GetTeamName());
    }

    public static Team GetTeam(PlayerController pc) {
        foreach(Team t in teams) {
            if (t.members.Contains(pc)) {
                return t;
            }
        }
        Team neutralTeam = teams[teams.Length-1];//The neutral team is always the last one
        JoinTeam(pc, neutralTeam);
        return neutralTeam;
    }

}