using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ResetSettings Variable", menuName = "Settings/Variables/ResetSettings")]
public class ResetSettingsParameter : FunctionParameter
{

    public override void Execute()
    {
        ResetParameters();
    }

}