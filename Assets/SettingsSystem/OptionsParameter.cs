using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Options Variable", menuName = "Settings/Variables/Options")]
public class OptionsParameter : Parameter<int>
{

    public Options Options => options;

    [SerializeField] private Options options;

    protected override int LoadValue()
    {
        return PlayerPrefs.GetInt(this.name);
    }

    protected override void SaveValue(int value)
    {
        PlayerPrefs.SetInt(this.name, value);
    }

}

[System.Serializable]
public struct Options
{

    public Option[] options;

    [System.Serializable]
    public struct Option
    {
        public string displayName;
    }

}