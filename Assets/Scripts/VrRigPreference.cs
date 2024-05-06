using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VrRigPreference : MonoBehaviour
{
    public static VrRigPreference Singleton;

    public GameObject XRRig;
    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public GameObject localPlayerGameObject;

    private void Awake()
    {
        Singleton = this;
    }
}
