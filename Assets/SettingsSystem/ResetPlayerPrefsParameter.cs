using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ResetPlayerPrefs Variable", menuName = "Settings/Variables/ResetPlayerPrefs")]
public class ResetPlayerPrefsParameter : FunctionParameter
{

    public override void Execute()
    {
        PlayerPrefs.DeleteAll();
        MapLoader.Instance.LoadMenu();
    }

}