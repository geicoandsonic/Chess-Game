using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pieceInformation : MonoBehaviour
{

    public bool tryPlacing(){
        if(transform.childCount == 0){ //square does not have a piece, can be placed
            return true;
        }
        return false;
    }

    public bool grabPiece(){
        if(transform.childCount > 0){ //square has a piece
            transform.GetChild(0).GetComponent<movePiece>().holdingPiece = true;
            transform.GetChild(0).SetParent(GameObject.FindWithTag("Player").transform);
            return true;
        }
        return false; //no piece
    }
}
