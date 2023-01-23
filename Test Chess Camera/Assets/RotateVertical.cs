using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateVertical : MonoBehaviour
{
    

    [SerializeField] private float speed;
    [SerializeField] private float minVal;
    [SerializeField] private float maxVal;
    [SerializeField] private float rotateY;

    private Quaternion currRot;
    private Quaternion newRot;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W)){
            currRot = transform.rotation;
            newRot = Quaternion.Euler(speed * Time.deltaTime, 0, 0);
            Quaternion tempRot = currRot * newRot;
            if (tempRot.eulerAngles.x <= maxVal || tempRot.eulerAngles.x >= minVal + 360)
            {
                transform.rotation = tempRot;
            }        
        }
        else if(Input.GetKey(KeyCode.S)){
            currRot = transform.rotation;
            newRot = Quaternion.Euler(-speed * Time.deltaTime, 0, 0);
            Quaternion tempRot = currRot * newRot;
            if (tempRot.eulerAngles.x >= minVal + 361 && tempRot.eulerAngles.x <= maxVal + 360)
            {
                transform.rotation = tempRot;
            } 
        }
    }
}
