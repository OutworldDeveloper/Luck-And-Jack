using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Music Volume Variable", menuName = "Settings/Variables/MusicVolume")]
public class MusicVolumeParameter : FloatParameter
{

    [SerializeField] private AudioMixer targetMixer;
    [SerializeField] private string targetParameter;

    protected override void OnGameStarted()
    {
        SetParameter();
    }

    protected override void OnValueChanged()
    {
        SetParameter();
    }

    private void SetParameter()
    {
        targetMixer.SetFloat(targetParameter, Mathf.Log10(GetValue()) * 20);
    }

}