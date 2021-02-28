using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ParameterEventBase : MonoBehaviour
{

    private void OnEnable() => BaseParameter.OnSettingsChanged += SettingsUpdated;

    private void OnDisable() => BaseParameter.OnSettingsChanged -= SettingsUpdated;

    private void Awake() => SettingsUpdated();

    protected abstract void SettingsUpdated();

}