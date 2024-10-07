using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private float rotateAmount = 1f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotateAmount * Time.deltaTime * 10f, Space.World);



    }

}