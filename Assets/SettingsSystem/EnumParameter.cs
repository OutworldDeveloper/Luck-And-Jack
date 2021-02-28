using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnumParameter<T> : Parameter<T> where T : System.Enum
{

    protected override T LoadValue()
    {
        return (T)(object)PlayerPrefs.GetInt(this.name);
    }

    protected override void SaveValue(T value)
    {
        PlayerPrefs.SetInt(this.name, (int)(object)value);
    }

}