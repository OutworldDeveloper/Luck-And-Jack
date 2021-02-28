using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SwapControls Variable", menuName = "Settings/Variables/SwapControlsFunction")]
public class SwapControlsParameter : FunctionParameter
{

    [SerializeField] private KeyCodeGroup[] keyCodeGroupsToSwap;

    public override void Execute()
    {
        foreach (var item in keyCodeGroupsToSwap)
            item.Swap();
    }

    [System.Serializable]
    private struct KeyCodeGroup
    {

        [SerializeField] private KeyCodeParameter keyCodeA;
        [SerializeField] private KeyCodeParameter keyCodeB;

        public void Swap()
        {
            KeyCode temp = keyCodeA.GetValue();
            keyCodeA.SetValue(keyCodeB.GetValue());
            keyCodeB.SetValue(temp);
        }

    }

}