using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(CanvasGroup))]
public class InteractionText : MonoBehaviour
{

    [SerializeField] private KeyCodeParameter interactionKey;

    private Text text;
    private CanvasGroup canvasGroup;
    private Interactable[] interactables;
    private Camera mainCamera;
    private Vector3 lastTextPosition;

    private void Awake()
    {
        text = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        interactables = FindObjectsOfType<Interactable>();
        mainCamera = Camera.main;
        canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        Interactable interactable = null;
        foreach (var item in interactables)
        {
            if (!item.IsInteractionAvaliable())
                continue;
            if (FlatVector.Distance(Player.Luck.transform.position, item.InteractionRangeCenterPoint) < item.InteractionRange)
            {
                interactable = item;
                break;
            }
        }

        if (interactable)
        {
            text.text = "Press " + interactionKey.GetValue().ToString() + " to " + interactable.InteractionText;
            lastTextPosition = interactable.InteractionTextPosition;
            if (canvasGroup.alpha < 1f)
                canvasGroup.alpha += Time.unscaledDeltaTime * 3f;
        }
        else
        {
            if (canvasGroup.alpha > 0f)
                canvasGroup.alpha -= Time.unscaledDeltaTime * 3f;
        }
        text.transform.position = mainCamera.WorldToScreenPoint(lastTextPosition);
    }

}