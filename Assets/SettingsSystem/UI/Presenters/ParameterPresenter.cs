using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ParameterPresenter<T> : BaseParameterPresenter where T : BaseParameter
{

    [SerializeField] private T manualInitialization;
    [SerializeField] private Text variableNameText;

    protected T settingsVariable;

    public override Type TargetType() => typeof(T);

    public override void Setup(BaseParameter settingsVariable)
    {
        this.settingsVariable = settingsVariable as T;
        variableNameText.text = GetPresenterNameText();
        Present(settingsVariable as T);
    }

    protected abstract void Present(T settingsVariable);

    protected virtual string GetPresenterNameText()
    {
        return settingsVariable.DisplayName;
    }

    protected void UpdateName()
    {
        variableNameText.text = GetPresenterNameText();
    }

    private void Start()
    {
        if (manualInitialization)
            Setup(manualInitialization);
        BaseParameter.OnSettingsChanged += BaseParameter_OnSettingsChanged;
    }

    private void OnDestroy()
    {
        BaseParameter.OnSettingsChanged -= BaseParameter_OnSettingsChanged;
    }

    private void BaseParameter_OnSettingsChanged()
    {
        Present(settingsVariable);
    }
}