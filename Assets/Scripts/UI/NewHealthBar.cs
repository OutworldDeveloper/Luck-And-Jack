using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class NewHealthBar : MonoBehaviour
{

    [SerializeField] private Image healthImage;
    [SerializeField] private Image delayedHealthImage;

    private float targetFillAmount;
    private bool recivedDamage;
    private bool recivedHeal;
    private Camera mainCamera;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        canvasGroup.alpha = 0f;
        Player.Luck.Damaged += Luck_Damaged;
        Player.Luck.Healed += Luck_Healed;
    }
    private void OnDestroy()
    {
        Player.Luck.Damaged -= Luck_Damaged;
        Player.Luck.Healed -= Luck_Healed;
    }

    private void Luck_Healed(int healAmount, int lastHealth)
    {
        targetFillAmount = (float)Player.Luck.Health / (float)Player.Luck.MaxHealth;
        delayedHealthImage.fillAmount = targetFillAmount;
        recivedDamage = false;
        recivedHeal = true;
    }

    private void Luck_Damaged(int damage, int lastHealth, FlatVector direction)
    {
        targetFillAmount = (float)Player.Luck.Health / (float)Player.Luck.MaxHealth;
        healthImage.fillAmount = targetFillAmount;
        recivedDamage = true;
        recivedHeal = false;

    }

    private void Update()
    {
        if (Player.Luck.Health != Player.Luck.MaxHealth)
        {
            if (canvasGroup.alpha < 1f)
                canvasGroup.alpha += Time.unscaledDeltaTime * 3f;
        }
        else
        {
            if (canvasGroup.alpha > 0f)
                canvasGroup.alpha -= Time.unscaledDeltaTime * 3f;
        }

        if (recivedDamage)
            delayedHealthImage.fillAmount = Mathf.MoveTowards(delayedHealthImage.fillAmount, targetFillAmount, Time.deltaTime * 1.5f);
        if(recivedHeal)
            healthImage.fillAmount = Mathf.MoveTowards(healthImage.fillAmount, targetFillAmount, Time.deltaTime * 1.5f);
    }

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(Player.Luck.transform.position + Vector3.up * 2.5f);
    }

}