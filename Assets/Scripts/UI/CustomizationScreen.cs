using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationScreen : MonoBehaviour
{

    [SerializeField] private UI_Button_Unlockable prefab_buttonUnlockable;
    [SerializeField] private Transform unlockablesParent;

    public void Start()
    {
        PlayerProfile.HatEquiped += PlayerProfile_HatEquiped;
        Refresh();
    }

    public void OnDestroy()
    {
        PlayerProfile.HatEquiped -= PlayerProfile_HatEquiped;
    }

    private void PlayerProfile_HatEquiped(object sender, JackHat e)
    {
        Refresh();
    }

    private void Refresh()
    {
        foreach (Transform transform in unlockablesParent)
            Destroy(transform.gameObject);
        foreach (JackHat hat in Unlockable.GetUnlockables<JackHat>())
            Instantiate(prefab_buttonUnlockable, unlockablesParent, false).Setup(hat);
    }

}