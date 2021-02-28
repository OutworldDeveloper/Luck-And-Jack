using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemode_Infinity : BaseGamemode
{

    private const string recordKey = "survivalRecord";

    public static int GetCurrentRecord() => PlayerPrefs.GetInt(recordKey);
    public static void SaveRecord(int record) => PlayerPrefs.SetInt(recordKey, record);

    [SerializeField] private NPC_Ghost prefab_ghost;

    public int LastRecord { get; private set; }

    protected override void Start()
    {
        base.Start();
        LastRecord = GetCurrentRecord();
        UpdateQuest("Find graves and extract the souls");
        MusicManager.Instance.PlayJackTheme();
    }

    protected override void OnGraveSaved()
    {
        ApplyRecord();

        if (Random.Range(0, 5) == 0)
            SpawnGhost();

        UpdateQuest("Souls extracted: " + GravesSaved);

        if (Graves.Length - GravesSaved > 3)
            return;

        Grave targetGrave = null;

        foreach (var grave in Graves)
        {
            if (!grave.IsSaved)
                continue;
            if (!targetGrave)
                targetGrave = grave;
            else
            {
                if(targetGrave.LastSavedTime > grave.LastSavedTime)
                    targetGrave = grave;
            }
        }

        targetGrave?.Respawn();
    }

    protected override void OnFirstRatEncounter()
    {
        MusicManager.Instance.PlayCombatPlaylist();
    }

    protected override void OnLuckDied()
    {
        MusicManager.Instance.PlayDeathMusic();
        ApplyRecord();
    }

    private void SpawnGhost()
    {
        MusicManager.Instance.PlayCombatPlaylist();
        UIController.Instance.ShowHelpText("You feel an evil presence...", 4f);
        FlatVector spawnPosition = RatSpawns[Random.Range(0, RatSpawns.Length)].transform.position;
        Instantiate(prefab_ghost, spawnPosition, Quaternion.identity);
    }

    private void ApplyRecord()
    {
        Unlockable.TryUnlockAll();
        if (LastRecord > GravesSaved)
            return;
        SaveRecord(GravesSaved);
    }

}