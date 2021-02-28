using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseParameterPresenter : MonoBehaviour
{
    public abstract Type TargetType();
    public abstract void Setup(BaseParameter settingsVariable);

}