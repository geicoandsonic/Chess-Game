using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHorizontal : MonoBehaviour
{
    

    [SerializeField] private float speed;
    [SerializeField] private float rotateX;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A)){
            rotateX = -speed * Time.deltaTime;
            transform.Rotate(0,rotateX,0);
        }
        else if(Input.GetKey(KeyCode.D)){
            rotateX = speed * Time.deltaTime;
            transform.Rotate(0,rotateX,0);
        }
    }
}
