using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unlockable : ScriptableObject
{

    private const string UnlockablesPath = "Unlockables";

    public static T[] GetUnlockables<T>() where T : Unlockable
    {
        return Resources.LoadAll<T>(UnlockablesPath);
    }

    public static T GetUnlockable<T>(string name) where T : Unlockable
    {
        foreach (var unlockable in GetUnlockables<T>())
        {
            if (unlockable.name == name)
                return unlockable;
        }
        return null;
    }

    public static void TryUnlockAll()
    {
        foreach (var item in Resources.LoadAll<Unlockable>(UnlockablesPath))
            item.TryUnlock();
    }

    [SerializeField] protected bool isUnlockedByDefault = false;
    [SerializeField] private Sprite sprite = null;

    public bool IsUnlocked() => isUnlockedByDefault || PlayerPrefs.GetInt(name + "unlocked") == 1;
    public Sprite Sprite => sprite;

    public void Unlock()
    {
        PlayerPrefs.SetInt(name + "unlocked", 1);
    }

    protected abstract void TryUnlock();

}