using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemode_Tutorial : BaseGamemode
{

    private const string tutorialCompletionKey = "tutorialCompleted";

    public static bool GetTutorialCompletionInfo() => PlayerPrefs.GetInt(tutorialCompletionKey) == 1;
    public static void CompleteTutorial() => PlayerPrefs.SetInt(tutorialCompletionKey, 1);

    [SerializeField] private KeyCodeParameter jackForward, jackBackward, jackLeft, jackRight, jackAttack;
    [SerializeField] private KeyCodeParameter luckForward, luckBackward, luckLeft, luckRight;
    [SerializeField] private NPC_Ghost prefab_tutorialGhost;
    [SerializeField] private GameObject tutorialWall;

    private bool hasSavedJack;

    public void JackSaved()
    {
        Player.Jack.SetPrologueSleeping(false);
        hasSavedJack = true;
        UpdateQuest(GetCurrentQuestText());
        string helpText = "Use " + 
            jackForward.GetValue() + " " + 
            jackBackward.GetValue() + " " + 
            jackLeft.GetValue() + " " + 
            jackRight.GetValue() + " to move Jack.\nUse " +
            jackAttack.GetValue() + " to attack.";
        UIController.Instance.ShowHelpText(helpText, 15f);
        tutorialWall.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        Player.Jack.SetPrologueSleeping(true);
        UpdateQuest(GetCurrentQuestText());
        string helpText = "Use " +
            luckForward.GetValue() + " " +
            luckBackward.GetValue() + " " +
            luckLeft.GetValue() + " " +
            luckRight.GetValue() + " to move Luck.";
        UIController.Instance.ShowHelpText(helpText, 10f);
        MusicManager.Instance.PlayJackTheme();
    }

    protected override void Update()
    {
        base.Update();
        if (hasSavedJack)
            return;
        if (FlatVector.Distance(tutorialWall.transform.position, Player.Luck.transform.position) < 3.4f)
            UIController.Instance.ShowHelpText("You need to wake up Jack first.", 0.5f);
    }

    private string GetCurrentQuestText()
    {
        if(!hasSavedJack)
            return "Find and wake up Jack";
        int gravesSaved = 0;
        foreach (var item in Graves)
            if (item.IsSaved) gravesSaved++;
        return "Extract souls " + gravesSaved + " / " + Graves.Length;
    }

    protected override void OnGraveSaved()
    {
        if (GravesSaved == 2 || GravesSaved == 4)
        {
            UIController.Instance.ShowHelpText("A ghost is near... The Jack's light will scare him away.", 15f);
            FlatVector spawnPosition = RatSpawns[UnityEngine.Random.Range(0, RatSpawns.Length)].transform.position;
            Instantiate(prefab_tutorialGhost, spawnPosition, Quaternion.identity);
            MusicManager.Instance.PlayCombatPlaylist();
        }

        if (GravesSaved == Graves.Length)
        {
            (UIController.Instance as UIControllerTutorial).EndTutorial();
            CompleteTutorial();
        }
        UpdateQuest(GetCurrentQuestText());
    }

    protected override void OnLuckDied()
    {
        MusicManager.Instance.PlayDeathMusic();
    }

    protected override void OnFirstRatEncounter()
    {
        MusicManager.Instance.PlayCombatPlaylist();
    }

}