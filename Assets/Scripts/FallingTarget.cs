using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTarget : MonoBehaviour
{
    public float destroyBelowY = -6f;

    void Update()
    {
        if (transform.position.y < destroyBelowY)
        {
            Destroy(gameObject);
        }
    }
}