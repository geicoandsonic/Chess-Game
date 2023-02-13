using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBishop : GeneralMovement
{
      void Awake()
    {
        moveSetup();
    }

    public override void moveSetup()
    {
        //diagonal
        addLongMovement(1, 1);
        addLongMovement(-1, 1);
        addLongMovement(1, -1);
        addLongMovement(-1, -1);
        //addShortMovement(0,3);
        //addShortMovement(0,-4);
    }

    public override void onMove()
    {
        listCleanup();
        //TODO: lose your ability to castle after first move
    }
}
