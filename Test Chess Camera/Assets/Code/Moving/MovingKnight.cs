using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingKnight : GeneralMovement
{
    int longDistance = 2; //The I part of the L
    int shortDistance = 1; //The _ part of the L
    void Awake()
    {
        moveSetup();
    }

    public void changeDistances(int shortDist,int longDist){
        longDistance = longDist;
        shortDistance = shortDist;
    }

    public override void moveSetup()
    {
        //Knight moves in L pattern, should have variables that can change distance if needed
        
        addShortMovement(longDistance,shortDistance);
        addShortMovement(longDistance,-shortDistance);
        addShortMovement(shortDistance,longDistance);
        addShortMovement(shortDistance,-longDistance);
        addShortMovement(-longDistance,shortDistance);
        addShortMovement(-longDistance,-shortDistance);
        addShortMovement(-shortDistance,longDistance);
        addShortMovement(-shortDistance,-longDistance);
        //addShortMovement(0,3);
        //addShortMovement(0,-4);
    }

    public override void onMove()
    {
        listCleanup();
        //TODO: lose your ability to castle after first move
    }
}
