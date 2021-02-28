using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParameterEventBool : ParameterEventBase
{

    public BoolEvent ParameterUpdated;

    [SerializeField] private BoolParameter targetParameter;

    protected override void SettingsUpdated() => ParameterUpdated.Invoke(targetParameter.GetValue());

}

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }