using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseParameter : ScriptableObject
{

    public static event Action OnSettingsChanged;

    private static SettingsInitializer settingsInitializer;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnBeforeSceneLoadRuntimeMethod()
    {
        if (settingsInitializer)
            return;
        settingsInitializer = new GameObject().AddComponent<SettingsInitializer>();
        settingsInitializer.OnStarted += SettingsInitializer_OnStarted;
    }

    private static void SettingsInitializer_OnStarted()
    {
        foreach (var group in ParametersGroup.GetGroups())
            foreach (var parameter in group.parameters)
                parameter.OnGameStarted();
    }

    public static void ResetParameters()
    {
        foreach (var group in ParametersGroup.GetGroups())
            foreach (var parameter in group.parameters)
                parameter.Reset();
    }

    [SerializeField] private string displayName;

    public string DisplayName => displayName;

    protected void SettingsChanged() => OnSettingsChanged?.Invoke();
    protected virtual void OnGameStarted() { }
    protected virtual void Reset() { }

}

public abstract class Parameter<T> : BaseParameter
{

    [SerializeField] private T defaultValue;

    private T cashedValue;
    private bool isCashed;

    public void SetValue(T value)
    {
        isCashed = false;
        SaveValue(value);
        SettingsChanged();
        OnValueChanged();
    }

    public T GetValue()
    {
        if (isCashed)
            return cashedValue;

        if (PlayerPrefs.HasKey(name))
        {
            cashedValue = LoadValue();
            isCashed = true;
            return cashedValue;
        }

        return defaultValue;
    }

    public void ResetValue() => SetValue(defaultValue);

    protected abstract T LoadValue();

    protected abstract void SaveValue(T value);

    protected virtual void OnValueChanged() { }

    protected override void Reset()
    {
        SetValue(defaultValue);
    }

}