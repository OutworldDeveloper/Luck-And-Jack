using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackReciver
{

    void SetKnockback(FlatVector direction, float force);

}