using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings Group", menuName = "Settings/Group")]
public class ParametersGroup : ScriptableObject
{

    private const string Path = "Settings/Groups";

    public static ParametersGroup[] GetGroups() => Resources.LoadAll<ParametersGroup>(Path);

    public string DisplayName;
    public List<BaseParameter> Parameters => parameters;

    [SerializeField] public List<BaseParameter> parameters;



}