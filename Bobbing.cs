using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    public bool Bob = false;
    public float Speed = 20f;
    public float Scale = 0.2f;

    Transform child;
    Vector3 offset = Vector3.zero;

    private void Start()
    {
        child = transform.GetChild(0).transform;
    }

    private void FixedUpdate()
    {
        if(Bob)
        {
            offset.y = Mathf.Sin(Time.fixedTime * Speed) * Scale;
            child.transform.position = transform.position + offset;
        }
    }
}
