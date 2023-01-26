using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingQueen : GeneralMovement
{
    void Awake()
    {
        moveSetup();
    }

    public override void moveSetup()
    {
        //cardinal
        for(int i = 0; i < 8; i++){ //Currently it should go at most 8 tiles
            addShortMovement(i,0);
            addShortMovement(-i,0);
            addShortMovement(0,i);
            addShortMovement(0,-i);
        }
        //diagonal
        for(int i = 0; i < 8; i++){ //Currently it should go at most 8 tiles
            addShortMovement(i,i);
            addShortMovement(i,-i);
            addShortMovement(-i,i);
            addShortMovement(-i,-i);
        }
        //addShortMovement(0,3);
        //addShortMovement(0,-4);
    }

    public override void onMove()
    {
        listCleanup();
        //TODO: lose your ability to castle after first move
    }
}
