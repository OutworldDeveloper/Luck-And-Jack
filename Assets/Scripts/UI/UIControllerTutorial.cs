using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControllerTutorial : UIController
{

    [SerializeField] private GameObject tutorialEndScreen = null;

    private UITutorialEnded tutorialEndedState;

    private bool tutorialEnded;

    public void Btn_EnterSurvival()
    {
        MapLoader.Instance.LoadSurvival();
    }

    public void EndTutorial()
    {
        tutorialEnded = true;
    }

    protected override void Awake()
    {
        base.Awake();
        tutorialEndedState = new UITutorialEnded(tutorialEndScreen);
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();
        stateMachine.AddGlobalTransition(tutorialEndedState, () => tutorialEnded && !Player.Luck.IsDead);
    }

    private class UITutorialEnded : State
    {

        private GameObject tutorialEndScreen;

        public UITutorialEnded(GameObject tutorialEndScreen)
        {
            this.tutorialEndScreen = tutorialEndScreen;
        }

        public override void OnEnter()
        {
            Cursor.visible = true;
            Time.timeScale = 0f;
            tutorialEndScreen.SetActive(true);
        }

        public override void OnExit()
        {
            Time.timeScale = 1f;
            tutorialEndScreen.SetActive(false);
        }

    }

}