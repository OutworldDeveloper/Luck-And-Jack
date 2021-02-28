using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FunctionParameter : BaseParameter
{

    [SerializeField] private string buttonText;

    public string ButtonText => buttonText;

    public abstract void Execute();

}