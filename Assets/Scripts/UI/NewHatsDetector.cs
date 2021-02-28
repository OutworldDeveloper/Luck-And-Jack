using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHatsDetector : MonoBehaviour
{

    [SerializeField] private GameObject indificator;

    private void Start()
    {
        indificator.SetActive(false);
        foreach (var item in Unlockable.GetUnlockables<JackHat>())
        {
            if (!item.IsInspected())
                indificator.SetActive(true);
        }
    }

}