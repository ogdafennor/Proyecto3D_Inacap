using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingCollectible : MonoBehaviour
{
    private float rotationSpeed = 50f;
    private float floatSpeed = 4f;
    private float floatAmplitude = 0.5f;
    private float bounceSpeed = 7f;
    private float bounceAmount = 0.05f;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        float yOffset =
            Mathf.Sin(Time.time * floatSpeed) * floatAmplitude +
            Mathf.Sin(Time.time * bounceSpeed) * bounceAmount;
        
        transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }
}
