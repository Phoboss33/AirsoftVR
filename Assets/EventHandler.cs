using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EventHandler : MonoBehaviour
{
    [SerializeField] XRGrabInteractable interactor;
    public delegate void SomethingHappend();
    public event SomethingHappend OnSomethingHappend;
    void Update()
    {
        //interactor.activated.AddListener();

    }
}