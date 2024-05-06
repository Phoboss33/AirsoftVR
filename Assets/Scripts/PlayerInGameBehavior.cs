using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInGameBehavior : MonoBehaviour
{
    public GameObject geoMesh;
    public void InviseMe()
    {
        geoMesh.SetActive(false);
    }
}
