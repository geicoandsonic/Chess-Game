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
        objMaterial.color = Color.blue;
    }
    void OnMouseExit(){
        objMaterial.color = Color.white;
    }

    void OnMouseDown()
    {
        Debug.Log("clicked");
    }

}
