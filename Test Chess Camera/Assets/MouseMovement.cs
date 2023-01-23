using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    Material objMaterial;

    void Start(){
        objMaterial = GetComponent<Renderer>().material;
    }

    void OnMouseOver(){
        //Debug.Log("Over an Object");
        objMaterial.color = Color.blue;
    }
    void OnMouseExit(){
        objMaterial.color = Color.white;
    }
}
