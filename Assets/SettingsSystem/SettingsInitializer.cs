using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsInitializer : MonoBehaviour
{

    public event Action OnStarted;

    private bool hasCalled;

    private void Start()
    {
        //if (hasCalled)
            //return;
        OnStarted?.Invoke();
        hasCalled = true;
    }

}