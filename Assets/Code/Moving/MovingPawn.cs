using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPawn : GeneralMovement
{
    [SerializeField] private bool isWhitePawn;
    void Awake(){
        moveSetup();
    }

    public override void moveSetup()
    {
        if(isWhitePawn){
            addShortMovement(1, 0);
            addShortMovement(2, 0);
        }
        else{
            addShortMovement(-1, 0);
            addShortMovement(-2, 0);
        }
        
        //shortMovements.AddFirst((1,0));
        //shortMovements.AddFirst((2,0));
    }

    public override void onMove()
    {
        if(isWhitePawn){
            removeShortMovement(2, 0);
        }
        else{
            removeShortMovement(-2, 0);
        }
        
        listCleanup();
        //shortMovements.Remove((2,0));
    }
}
