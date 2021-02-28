using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bool Variable", menuName = "Settings/Variables/Bool")]
public class BoolParameter : Parameter<bool>
{

    protected override bool LoadValue()
    {
        return PlayerPrefs.GetInt(this.name) != 0;
    }

    protected override void SaveValue(bool value)
    {
        PlayerPrefs.SetInt(this.name, (value ? 1 : 0));
    }

}