using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VibratonManager : MonoBehaviour
{
    public InputDevice leftController;
    public InputDevice rightController;

    // Start is called before the first frame update
    void Start()
    {
        leftController.TryGetHapticCapabilities(out var capabilities);

        Debug.Log(capabilities.supportsImpulse);

        Debug.Log(leftController.ToString());
        //Debug.Log(leftController.SendHapticImpulse(0.9f, 10));
    }
}
