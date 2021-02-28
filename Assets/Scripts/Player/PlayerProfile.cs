using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{

    private const string JackHatSlotKey = "jackHat";
    private const string gamejamVersion = "brackeysgamejam1";

    public static PlayerProfile Instance { get; private set; }
    public static event EventHandler<JackHat> HatEquiped;

    public static void Equip(JackHat item)
    {
        PlayerPrefs.SetString(JackHatSlotKey, item.name);
        HatEquiped?.Invoke(null, item);
    }

    public static JackHat GetEquipedJackHat()
    {
        if (!PlayerPrefs.HasKey(JackHatSlotKey))
            return Unlockable.GetUnlockables<JackHat>()[0];
        return Unlockable.GetUnlockable<JackHat>(PlayerPrefs.GetString(JackHatSlotKey));
    }

    [SerializeField] private Unlockable gamejamUnlockable;

    private bool isRewardUnlocked;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        if (PlayerPrefs.GetInt(gamejamVersion) == 1)
        {
            isRewardUnlocked = true;
            gamejamUnlockable.Unlock();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (isRewardUnlocked)
        {
            gamejamUnlockable.Unlock();
            PlayerPrefs.SetInt(gamejamVersion, 1);
        }
    }

}