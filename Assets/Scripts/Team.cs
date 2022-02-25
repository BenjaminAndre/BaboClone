using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    [SerializeField]
    private string teamName;

    [SerializeField]
    private Material teamMaterial;

    public List<PlayerController> members = new List<PlayerController>();

    public Material GetTeamMaterial() {
        return teamMaterial;
    }

    public string GetTeamName() {
        return teamName;
    }

    // Will get more complicated with gamemodes
    public bool CanHurt(Team other) {
        return other != this;
    }

}
