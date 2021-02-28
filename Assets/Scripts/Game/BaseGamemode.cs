using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGamemode : MonoBehaviour
{

    public static BaseGamemode Instance { get; private set; }

    [SerializeField] private AnimationCurve ratsSpawnIncreasingCurve;
    [SerializeField] protected NPC_Rat prefab_defaultRat;

    public event Action<string> QuestUpdated;
    public string CurrentQuest { get; private set; }
    public RatSpawnPoint[] RatSpawns { get; private set; }
    public Grave[] Graves { get; private set; }
    public int GravesSaved { get; private set; }
    public int RatsKilled { get; private set; }
    public int RatsAlive { get; private set; }
    public bool HasSeenRat { get; protected set; }

    private bool ratSpawnClose;

    public RatSpawnPoint GetClosestRatSpawnPoint(FlatVector location)
    {
        RatSpawnPoint currentTarget = null;
        foreach (var item in Instance.RatSpawns)
        {
            if (!currentTarget)
                currentTarget = item;
            if (FlatVector.Distance(location, currentTarget.transform.position) >
                FlatVector.Distance(location, item.transform.position))
                currentTarget = item;
        }
        return currentTarget;
    }

    protected virtual void Awake()
    {
        Instance = this;
        RatSpawns = FindObjectsOfType<RatSpawnPoint>();
        Graves = FindObjectsOfType<Grave>();
    }

    protected virtual void Start()
    {
        Grave.OnGraveSaved += Grave_OnGraveSaved;
        Player.Luck.Died += Luck_Died;
    }

    private void OnDestroy()
    {
        Grave.OnGraveSaved -= Grave_OnGraveSaved;
        Player.Luck.Died -= Luck_Died;
    }

    protected virtual void Update()
    {
        if (HasSeenRat)
            return;
        foreach (var item in FindObjectsOfType<NPC_Rat>())
        {
            if (FlatVector.Distance(Player.Luck.transform.position, item.transform.position) < 15f ||
                FlatVector.Distance(Player.Jack.transform.position, item.transform.position) < 15f)
            {
                OnFirstRatEncounter();
                HasSeenRat = true;
            }
        }
    }

    protected void UpdateQuest(string quest)
    {
        CurrentQuest = quest;
        QuestUpdated?.Invoke(quest);
    }

    protected void SpawnRat()
    {
        FlatVector spawnPosition;
        if (ratSpawnClose)
        {
            spawnPosition = GetClosestRatSpawnPoint(Player.Luck.transform.position).transform.position;
            ratSpawnClose = false;
        }
        else
        {
            spawnPosition = RatSpawns[UnityEngine.Random.Range(0, RatSpawns.Length)].transform.position;
            ratSpawnClose = true;
        }
        NPC_Rat rat = Instantiate(prefab_defaultRat, spawnPosition, Quaternion.identity);
        rat.Died += Rat_Died;
        RatsAlive++;
    }

    private void Rat_Died()
    {
        RatsAlive--;
        RatsKilled++;
        OnRatDied();
        SpawnRats();
        OnFirstRatEncounter();
        HasSeenRat = true;
    }

    private void Grave_OnGraveSaved()
    {
        GravesSaved++;
        OnGraveSaved();
        SpawnRats();
    }

    private void Luck_Died()
    {
        OnLuckDied();
    }

    protected void SpawnRats()
    {
        int ratsMaxCount = Mathf.FloorToInt(ratsSpawnIncreasingCurve.Evaluate(GravesSaved));
        int toSpawn = ratsMaxCount - RatsAlive;
        for (int i = 0; i < toSpawn; i++)
            SpawnRat();
    }

    protected virtual void OnGraveSaved() { }
    protected virtual void OnRatDied() { }
    protected virtual void OnLuckDied() { }
    protected virtual void OnFirstRatEncounter() { }

}