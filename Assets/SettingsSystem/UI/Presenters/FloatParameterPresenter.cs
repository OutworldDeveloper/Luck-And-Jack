using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatParameterPresenter : ParameterPresenter<FloatParameter>
{

    [SerializeField] private Slider slider;

    protected override void Present(FloatParameter settingsVariable)
    {
        slider.minValue = settingsVariable.MinValue;
        slider.maxValue = settingsVariable.MaxValue;
        slider.value = settingsVariable.GetValue();
    }

    private void OnEnable() => slider.onValueChanged.AddListener(OnValueChanged);

    private void OnDisable() => slider.onValueChanged.RemoveListener(OnValueChanged);

    public void OnValueChanged(float value)
    {
        settingsVariable.SetValue(value);
        UpdateName();
    }

    protected override string GetPresenterNameText()
    {
        return base.GetPresenterNameText() + " " + 
            (settingsVariable.GetValue() / settingsVariable.MaxValue * 100f).ToString("0") + "%";
    }

}
