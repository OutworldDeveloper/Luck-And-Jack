using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }
    public static Luck Luck => Instance.luck;
    public static Jack Jack => Instance.jack;

    [SerializeField] private Luck luck;
    [SerializeField] private Jack jack;
    [SerializeField] private GameObject virtualCamera_default;
    [SerializeField] private GameObject virtualCamera_luck;

    [SerializeField] private KeyCodeParameter luck_moveForward, luck_moveBackward, luck_moveLeft, luck_moveRight, luck_interact;
    [SerializeField] private KeyCodeParameter jack_moveForward, jack_moveBackward, jack_moveLeft, jack_moveRight, jack_attack;
    [SerializeField] private KeyCodeParameter takeScreenshot;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        FlatVector luckInputVector = new FlatVector();
        if (Input.GetKey(luck_moveForward.GetValue()))
            luckInputVector.z += 1;
        if (Input.GetKey(luck_moveBackward.GetValue()))
            luckInputVector.z -= 1;
        if (Input.GetKey(luck_moveLeft.GetValue()))
            luckInputVector.x -= 1;
        if (Input.GetKey(luck_moveRight.GetValue()))
            luckInputVector.x += 1;
        luck.InputVector = luckInputVector.normalized;

        FlatVector jackInputVector = new FlatVector();
        if (Input.GetKey(jack_moveForward.GetValue()))
            jackInputVector.z += 1;
        if (Input.GetKey(jack_moveBackward.GetValue()))
            jackInputVector.z -= 1;
        if (Input.GetKey(jack_moveLeft.GetValue()))
            jackInputVector.x -= 1;
        if (Input.GetKey(jack_moveRight.GetValue()))
            jackInputVector.x += 1;
        jack.InputVector = jackInputVector.normalized;

        if (Input.GetKeyDown(jack_attack.GetValue()))
            jack.TryAttack();

        //if (Input.GetKeyDown(KeyCode.Space))
        //luck.TryDodge();

        if (Input.GetKeyDown(luck_interact.GetValue()))
            luck.TryInteract();

        //if (Input.GetKeyDown(KeyCode.Space))
        //luck.TryVault();

        float distance = FlatVector.Distance(luck.transform.position, jack.transform.position);

        if (virtualCamera_luck.activeSelf)
        {
            if (distance < 7.5f)
                virtualCamera_luck.SetActive(false);
        }
        else
        {
            if (distance > 9.5f)
                virtualCamera_luck.SetActive(true);
        }

        
        if (Input.GetKeyDown(takeScreenshot.GetValue()))
        {
            string screenshotName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png";

            string path = System.IO.Directory.GetParent(Application.dataPath) + "/Screenshots/"; 

            System.IO.Directory.CreateDirectory(path);

            ScreenCapture.CaptureScreenshot(path + screenshotName);

            UIController.Instance?.ShowHelpText("Saved screenshot as " + screenshotName, 2f);
        }
        
    }

}