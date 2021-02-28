using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Float Variable", menuName = "Settings/Variables/Float")]
public class FloatParameter : Parameter<float>
{

    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;

    public float MinValue => minValue;
    public float MaxValue => maxValue;

    protected override float LoadValue()
    {
        return PlayerPrefs.GetFloat(name);
    }

    protected override void SaveValue(float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }

}