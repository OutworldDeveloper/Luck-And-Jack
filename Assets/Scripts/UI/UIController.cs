using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : StateMachineBehaviour
{

    public static UIController Instance { get; private set; }

    [SerializeField] private GameObject hud = null;
    [SerializeField] private GameObject deathScreen = null;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject settingsScreen = null;
    [SerializeField] private GameObject notesScreen = null;
    [SerializeField] private Text helpText = null;
    [SerializeField] private CanvasGroup helpTextCanvasGroup = null;
    [SerializeField] private Text noteReaderHeader = null;
    [SerializeField] private Text noteReaderContent = null;
    [SerializeField] private SoundPlayer noteSoundPlayer = null;

    private Trigger switchPauseTrigger;
    private Trigger switchSettingsTrigger;
    private Trigger switchNoteReaderTrigger;

    private UIGame gameState;
    private UIDeath deathState;
    private UIPause pauseState;
    private UISettings settingsState;
    private UINoteReader noteReaderState;

    private float helpTextEndTime;

    protected override State initialState => gameState;

    public void ShowHelpText(string text, float duration)
    {
        helpText.text = text;
        helpTextEndTime = Time.time + duration;
    }

    public void ShowNote(Note note)
    {
        switchNoteReaderTrigger.SetActive();
        noteReaderHeader.text = note.Title;
        noteReaderContent.text = note.Content;
    }

    public void Btn_CloseNote()
    {
        switchNoteReaderTrigger.SetActive();
    }

    public void Btn_Respawn()
    {
        MapLoader.Instance.ReloadCurrentScene();
    }

    public void Btn_Exit()
    {
        MapLoader.Instance.LoadMenu();
    }

    public void Btn_Continue()
    {
        switchPauseTrigger.SetActive();
    }

    public void Btn_OpenSettingsMenu()
    {
        switchSettingsTrigger.SetActive();
    }

    protected virtual void Awake()
    {
        Instance = this;
        switchPauseTrigger = stateMachine.CreateTrigger();
        switchSettingsTrigger = stateMachine.CreateTrigger();
        switchNoteReaderTrigger = stateMachine.CreateTrigger();
        gameState = new UIGame(hud);
        deathState = new UIDeath(deathScreen);
        pauseState = new UIPause(pauseMenu);
        settingsState = new UISettings(settingsScreen);
        noteReaderState = new UINoteReader(notesScreen, noteSoundPlayer);
    }

    protected override void SetupStateMachine()
    {
        stateMachine.AddTransition(gameState, deathState, () => Player.Luck.IsDead);
        stateMachine.AddTransition(gameState, pauseState, () => switchPauseTrigger.IsActive);
        stateMachine.AddTransition(gameState, noteReaderState, () => switchNoteReaderTrigger.IsActive);

        stateMachine.AddTransition(pauseState, gameState, () => switchPauseTrigger.IsActive);
        stateMachine.AddTransition(pauseState, settingsState, () => switchSettingsTrigger.IsActive);

        stateMachine.AddTransition(settingsState, pauseState, () => switchSettingsTrigger.IsActive);

        stateMachine.AddTransition(noteReaderState, gameState, () => switchNoteReaderTrigger.IsActive);
    }

    protected override void Update()
    {
        base.Update();
        if(Time.time < helpTextEndTime)
        {
            if (helpTextCanvasGroup.alpha < 1f)
                helpTextCanvasGroup.alpha += Time.unscaledDeltaTime;
        }
        else
        {
            if (helpTextCanvasGroup.alpha > 0f)
                helpTextCanvasGroup.alpha -= Time.unscaledDeltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switchPauseTrigger.SetActive();
            switchNoteReaderTrigger.SetActive();
        }
    }

    private class UIGame : State
    {

        private GameObject hudScreen;

        public UIGame(GameObject hudScreen)
        {
            this.hudScreen = hudScreen;
        }

        public override void OnEnter()
        {
            Cursor.visible = false;
            Time.timeScale = 1f;
            hudScreen.SetActive(true);
        }

        public override void OnExit()
        {
            Cursor.visible = true;
            hudScreen.SetActive(false);
        }

    }

    private class UIDeath : State
    {

        private GameObject deathScreen;

        public UIDeath(GameObject deathScreen)
        {
            this.deathScreen = deathScreen;
        }

        public override void OnEnter()
        {
            Cursor.visible = true;
            deathScreen.SetActive(true);
        }

        public override void OnExit()
        {
            deathScreen.SetActive(false);
        }

    }

    private class UIPause : State
    {

        private GameObject pauseMenu;

        public UIPause(GameObject pauseMenu)
        {
            this.pauseMenu = pauseMenu;
        }

        public override void OnEnter()
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            pauseMenu.SetActive(true);
        }

        public override void OnExit()
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }

    }

    private class UISettings : State
    {

        private GameObject settingsMenu;

        public UISettings(GameObject settingsMenu)
        {
            this.settingsMenu = settingsMenu;
        }

        public override void OnEnter()
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            settingsMenu.SetActive(true);
        }

        public override void OnExit()
        {
            Time.timeScale = 1f;
            settingsMenu.SetActive(false);
        }

    }

    private class UINoteReader : State
    {

        private GameObject notesScreen;
        private SoundPlayer noteSoundPlayer;

        public UINoteReader(GameObject notesScreen, SoundPlayer noteSoundPlayer)
        {
            this.notesScreen = notesScreen;
            this.noteSoundPlayer = noteSoundPlayer;
        }

        public override void OnEnter()
        {
            noteSoundPlayer.PlaySound();
            Time.timeScale = 0f;
            Cursor.visible = true;
            notesScreen.SetActive(true);
        }

        public override void OnExit()
        {
            Time.timeScale = 1f;
            notesScreen.SetActive(false);
        }

    }

}