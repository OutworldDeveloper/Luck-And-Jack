using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button_Unlockable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Button button;
    [SerializeField] private Image unlockableImage;
    [SerializeField] private Text description;

    public Button Button { get; private set; }

    private JackHat targetHat;

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.gameObject.SetActive(false);
    }

    public void Setup(Unlockable unlockable)
    {
        targetHat = unlockable as JackHat;
        bool isUnlocked = unlockable.IsUnlocked();
        unlockableImage.sprite = unlockable.Sprite;
        unlockableImage.color = isUnlocked ? Color.white : Color.black;
        if (isUnlocked)
            Button.onClick.AddListener(() => PlayerProfile.Equip(unlockable as JackHat));
    }    

    private void Awake()
    {
        Button = button;
    }

    private void Start()
    {
        description.gameObject.SetActive(false);
        if (targetHat.IsUnlocked())
            description.text = targetHat.DisplayName;
        else
        {
            description.text = targetHat.SurvivalModeRecordUnlockRequirement == -1 ? "Secret Hat" : 
                "Survival Record to unlock: " + Gamemode_Infinity.GetCurrentRecord() + "/" + targetHat.SurvivalModeRecordUnlockRequirement;
        }
    }

}