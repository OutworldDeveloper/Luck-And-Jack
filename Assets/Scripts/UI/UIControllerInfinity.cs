using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerInfinity : UIController
{
   
    [SerializeField] private Text deathInfoText;

    protected override void Start()
    {
        base.Start();
        Player.Luck.Died += Luck_Died;
    }

    private void OnDestroy()
    {
        Player.Luck.Died -= Luck_Died;
    }

    private void Luck_Died()
    {
        Gamemode_Infinity gamemode = BaseGamemode.Instance as Gamemode_Infinity;
        int lastRecord = gamemode.LastRecord;
        int newRecord = gamemode.GravesSaved;
        deathInfoText.text = lastRecord < newRecord ?
            "New record! You extracted " + newRecord + " souls." :
            "You extracted " + newRecord + " souls. Your last record is " + lastRecord + ".";
    }

}