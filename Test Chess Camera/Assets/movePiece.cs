using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePiece : MonoBehaviour
{
    public bool holdingPiece;
    // Update is called once per frame
    void Update()
    {
        while(holdingPiece){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
            transform.position = mousePosition;
        }
    }

    public void placePiece(Transform newParent){
        transform.SetParent(newParent.transform,false);
    }
}
