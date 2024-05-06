using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KostyileScript : MonoBehaviour
{
    public GameObject localSwat;

    static public GameObject localSwaatAgaOgo;

    private void Start()
    {
        localSwaatAgaOgo = localSwat;
    }
}
