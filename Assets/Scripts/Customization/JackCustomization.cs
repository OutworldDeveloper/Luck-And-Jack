using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackCustomization : MonoBehaviour
{

    [SerializeField] private Transform headBone;
    [SerializeField] private bool isSleepingByDefault;

    private JackHatObject currentHat;
    private bool isSleeping;

    public void SetSleeping(bool sleep)
    {
        isSleeping = sleep;
        currentHat?.SetSleeping(sleep);
    }

    private void Start()
    {
        PlayerProfile.HatEquiped += PlayerProfile_HatEquiped;
        EquipHat(PlayerProfile.GetEquipedJackHat());
        if(isSleepingByDefault) 
            SetSleeping(true);
    }

    private void OnDestroy()
    {
        PlayerProfile.HatEquiped -= PlayerProfile_HatEquiped;
    }

    private void PlayerProfile_HatEquiped(object sender, JackHat e)
    {
        EquipHat(e);
    }

    private void EquipHat(JackHat hat)
    {
        if (currentHat)
            Destroy(currentHat.gameObject);
        currentHat = Instantiate(hat.HatPrefab, headBone);
        currentHat.transform.localPosition = Vector3.zero;
        currentHat.SetSleeping(isSleeping);
    }

}