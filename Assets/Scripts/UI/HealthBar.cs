using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Transform healthPrefab;

    private int lastHealth;

    private void Start()
    {
        Player.Luck.Damaged += Luck_Damaged;
        Player.Luck.Healed += Luck_Healed;
        lastHealth = Player.Luck.Health;
        Refresh();
    }

    private void OnDestroy()
    {
        Player.Luck.Damaged -= Luck_Damaged;
        Player.Luck.Healed -= Luck_Healed;
    }

    private void Luck_Healed(int healAmount, int lastHealth)
    {
        int heartsToAdd = Player.Luck.Health - lastHealth;
        for (int i = 0; i < heartsToAdd; i++)
            Instantiate(healthPrefab, transform);
    }

    private void Luck_Damaged(int damage, int lastHealth, FlatVector direction)
    {
        int heartsToDestroy = lastHealth - Player.Luck.Health;
        for (int i = 0; i < heartsToDestroy; i++)
        {
            Transform t = transform.GetChild(transform.childCount - 1);
            t.GetComponent<Animator>().SetTrigger("Death");
            Destroy(t.gameObject, 2f);
        }
    }

    private void Refresh()
    {
        foreach (Transform item in transform)
            Destroy(item.gameObject);
        for (int i = 0; i < Player.Luck.Health; i++)
            Instantiate(healthPrefab, transform);
    }

}