using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JustTextParameterPresenter : ParameterPresenter<JustTextParameter>
{

    protected override void Present(JustTextParameter settingsVariable) { }

    protected override string GetPresenterNameText()
    {
        return settingsVariable.DisplayName;
    }

}