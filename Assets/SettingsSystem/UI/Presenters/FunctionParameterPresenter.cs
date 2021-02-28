using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionParameterPresenter : ParameterPresenter<FunctionParameter>
{

    [SerializeField] private Button executeButton;
    
    protected override void Present(FunctionParameter settingsVariable) 
    {
        executeButton.GetComponentInChildren<Text>().text = settingsVariable.ButtonText;
    }

    private void OnEnable() => executeButton.onClick.AddListener(OnButtonPressed);

    private void OnDisable() => executeButton.onClick.RemoveListener(OnButtonPressed);

    public void OnButtonPressed()
    {
        settingsVariable.Execute();
    }
}