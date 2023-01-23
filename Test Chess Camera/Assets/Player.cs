using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private bool holdingPiece;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {  
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
            RaycastHit hit;  
            if (Physics.Raycast(ray, out hit)) {  
                if (hit.transform.tag == "BoardSquare") {  
                    Debug.Log("Hit a square");
                    if(holdingPiece){
                        bool canPlace = hit.transform.GetComponent<pieceInformation>().tryPlacing();
                        if(canPlace){
                            transform.GetChild(0).GetComponent<movePiece>().placePiece(hit.transform);
                        }
                    }
                    else{
                        hit.transform.GetComponent<pieceInformation>().grabPiece();
                    }                   
                }  
            }  
        }
    }

}
