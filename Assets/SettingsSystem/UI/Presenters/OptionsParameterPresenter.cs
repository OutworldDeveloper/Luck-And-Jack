using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsParameterPresenter : ParameterPresenter<OptionsParameter>
{

    [SerializeField] private Dropdown dropdown;

    protected override void Present(OptionsParameter settingsVariable)
    {
        dropdown.ClearOptions();
        foreach (var item in settingsVariable.Options.options)
        {
            Dropdown.OptionData _optionData = new Dropdown.OptionData();
            _optionData.text = item.displayName;
            dropdown.options.Add(_optionData);
        }
        dropdown.value = settingsVariable.GetValue();
    }

    private void OnValueChanged(int value) => settingsVariable.SetValue(value);

    private void OnEnable() => dropdown.onValueChanged.AddListener(OnValueChanged);

    private void OnDisable() => dropdown.onValueChanged.RemoveListener(OnValueChanged);

}