using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{

    [SerializeField] private Image minimapImage;
    [SerializeField] private Text debugText;
    [SerializeField] private Vector2 extraOffset;

    private Transform target;

    private void Start()
    {
        target = Player.Luck.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            minimapImage.gameObject.SetActive(!minimapImage.gameObject.activeSelf);
    }

    private void LateUpdate()
    {
        Vector2 cameraPosition = new Vector2(target.position.x, target.position.z);
        Vector2 offset = new Vector2(cameraPosition.x, cameraPosition.y);
        minimapImage.transform.localPosition = -(offset * 10f);
        debugText.text = "target " + cameraPosition + " | minimap " + offset;
    }

}