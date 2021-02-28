using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Note")]
public class Note : ScriptableObject
{

    [SerializeField] private string title;
    [Multiline(5)]
    [SerializeField] private string content;

    public string Title => title;
    public string Content => content;
    
}