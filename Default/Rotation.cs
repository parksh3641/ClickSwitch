using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private float speed = 25f;
    private void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
