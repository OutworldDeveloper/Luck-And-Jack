using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{

    private void Start()
    {
        ScreenCapture.CaptureScreenshot("test.png");
    }

}