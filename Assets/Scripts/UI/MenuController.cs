using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : StateMachineBehaviour
{

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject survivalGamemodeButton;
    [SerializeField] private GameObject hatsScreen;
    [SerializeField] private Text infinityRecordText;
    [SerializeField] private GameObject jackVirtualCamera;
    [SerializeField] private GameObject newHatsIndificator;

    private Trigger switchSettingsTrigger;
    private Trigger switchHatsTrigger;

    private Menu menuState;
    private HatsState hatsState;
    private Settings settingsState;

    protected override State initialState => menuState;

    public void Btn_StartTutorial()
    {
        MapLoader.Instance.LoadTutorial();
    }

    public void Btn_StartSurvival()
    {
        MapLoader.Instance.LoadSurvival();
    }

    public void Btn_SwitchHatsMenu()
    {
        switchHatsTrigger.SetActive();
    }

    public void Btn_Exit()
    {
        Application.Quit();
    }

    public void Btn_OpenSettingsMenu()
    {
        switchSettingsTrigger.SetActive();
    }

    private void Awake()
    {
        switchSettingsTrigger = stateMachine.CreateTrigger();
        switchHatsTrigger = stateMachine.CreateTrigger();
        menuState = new Menu(menu, newHatsIndificator);
        hatsState = new HatsState(hatsScreen, jackVirtualCamera);
        settingsState = new Settings(settingsScreen);
    }

    protected override void Start()
    {
        base.Start();
        int record = Gamemode_Infinity.GetCurrentRecord();
        infinityRecordText.gameObject.SetActive(record > 0);
        infinityRecordText.text = "survival record: " + record;
        survivalGamemodeButton.SetActive(Gamemode_Tutorial.GetTutorialCompletionInfo());
        MusicManager.Instance.PlayMenuMusic();
    }

    protected override void SetupStateMachine()
    {
        stateMachine.AddTransition(menuState, settingsState, () => switchSettingsTrigger.IsActive);
        stateMachine.AddTransition(settingsState, menuState, () => switchSettingsTrigger.IsActive);
        stateMachine.AddTransition(menuState, hatsState, () => switchHatsTrigger.IsActive);
        stateMachine.AddTransition(hatsState, menuState, () => switchHatsTrigger.IsActive);
    }

    private class Menu : State
    {

        private GameObject menu;
        private GameObject newHatsIndificator;

        public Menu(GameObject menu, GameObject newHatsIndificator)
        {
            this.menu = menu;
            this.newHatsIndificator = newHatsIndificator;
        }

        public override void OnEnter()
        {
            menu.gameObject.SetActive(true);
            Time.timeScale = 1f;
            Cursor.visible = true;
            newHatsIndificator.SetActive(false);
            foreach (var item in Unlockable.GetUnlockables<JackHat>())
            {
                if (!item.IsInspected())
                    newHatsIndificator.SetActive(true);
            }
        }

        public override void OnExit()
        {
            menu.gameObject.SetActive(false);
        }

    }

    private class HatsState : State
    {

        private GameObject hatsScreen;
        private GameObject jackVirtualCamera;

        public HatsState(GameObject hatsScreen, GameObject jackVirtualCamera)
        {
            this.hatsScreen = hatsScreen;
            this.jackVirtualCamera = jackVirtualCamera;
        }

        public override void OnEnter()
        {
            hatsScreen.SetActive(true);
            jackVirtualCamera.SetActive(true);
            Cursor.visible = true;
            foreach (var item in Unlockable.GetUnlockables<JackHat>())
                item.SetInspected();
        }

        public override void OnExit()
        {
            hatsScreen.SetActive(false);
            jackVirtualCamera.SetActive(false);
        }

    }

    private class Settings : State
    {

        private GameObject settings;

        public Settings(GameObject settings)
        {
            this.settings = settings;
        }

        public override void OnEnter()
        {
            settings.gameObject.SetActive(true);
            Cursor.visible = true;
        }

        public override void OnExit()
        {
            settings.gameObject.SetActive(false);
        }

    }

}