using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPawn : GeneralMovement
{
    [SerializeField] private bool isWhitePawn;
    public int strikingDistance = 1;
    void Awake(){
        moveSetup();
    }

    public override void moveSetup()
    {
        if(isWhitePawn){
            addShortMovement(1, 0);
            addShortMovement(2, 0);
            addSpecialMovement(1,1);
            addSpecialMovement(1,-1);
            //addShortMovement(2,1); //Pawn has not moved yet, and enemy unit is in range
            //addShortMovement(2,-1); //not sure this is allowed tbh
            //add E.P here later, probably under a "special cases". We need to track the enemy's last turn movement.
        }
        else{
            addShortMovement(-1, 0);
            addShortMovement(-2, 0);
            addSpecialMovement(-1,1);
            addSpecialMovement(-1,-1);
            //addShortMovement(-2,1); //Pawn has not moved yet, and enemy unit is in range
            //addShortMovement(-2,-1);
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
