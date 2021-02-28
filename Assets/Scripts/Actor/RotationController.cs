using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{

    [SerializeField] private float initialRotationSpeed = 900f;
    [SerializeField] private float rotationSpeed = 1200f;
    [SerializeField] private float acceleration = 1500f;

    private Quaternion desiredRotation;
    private float currentRotationSpeed = 0f;

    public void SetDesiredRotation(Quaternion rotation)
    {
        desiredRotation = rotation;
    }

    public void LookAt(FlatVector targetPosition)
    {
        FlatVector direction = targetPosition - transform.FlatPosition();
        LookIn(direction);
    }

    public void LookIn(FlatVector direction)
    {
        if (direction != FlatVector.zero)
            desiredRotation = Quaternion.LookRotation(direction.Vector3, Vector3.up);
    }

    private void Start()
    {
        currentRotationSpeed = initialRotationSpeed;
    }

    private void Update()
    {
        if (transform.rotation == desiredRotation)
        {
            currentRotationSpeed = initialRotationSpeed;
            return;
        }
        currentRotationSpeed = Mathf.Min(rotationSpeed, currentRotationSpeed + acceleration * Time.deltaTime);
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, desiredRotation, currentRotationSpeed * Time.deltaTime);
    }

}