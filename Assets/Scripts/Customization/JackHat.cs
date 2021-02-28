using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jack's hat", menuName = "Customization/JackHat")]
public class JackHat : Unlockable
{

    [SerializeField] private string displayName = "New Hat";
    [SerializeField] private JackHatObject hatPrefab;
    [SerializeField] private int survivalModeRecordUnlockRequirement = -1;

    public string DisplayName => displayName;
    public JackHatObject HatPrefab => hatPrefab;
    public int SurvivalModeRecordUnlockRequirement => survivalModeRecordUnlockRequirement;

    public bool IsInspected()
    {
        if (isUnlockedByDefault)
            return true;
        if (!IsUnlocked())
            return true;
        return PlayerPrefs.GetInt(name + "_seen") == 1;
    }

    public void SetInspected()
    {
        if (!IsUnlocked())
            return;
        PlayerPrefs.SetInt(name + "_seen", 1); 
    }

    protected override void TryUnlock()
    {
        if (survivalModeRecordUnlockRequirement == -1)
            return;
        if (Gamemode_Infinity.GetCurrentRecord() >= survivalModeRecordUnlockRequirement)
            Unlock();
    }

}