using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(Animator))]
public class CurrentQuestText : MonoBehaviour
{

    private Text text;
    private Animator animator;

    private void Awake()
    {
        text = GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        BaseGamemode.Instance.QuestUpdated += QuestUpdated;
        text.text = BaseGamemode.Instance.CurrentQuest;
    }

    private void OnDestroy()
    {
        BaseGamemode.Instance.QuestUpdated -= QuestUpdated;
    }

    private void QuestUpdated(string obj)
    {
        text.text = obj;
        animator.SetTrigger("Changed");
    }

}