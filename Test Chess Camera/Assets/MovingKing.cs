using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingKing : GeneralMovement
{

    void Awake()
    {
        //ghostTile = GameObject.FindWithTag("ghostTile");
        moveSetup();
    }

    public override void moveSetup()
    {
        //cardinal
        addShortMovement(1, 0);
        addShortMovement(0, 1);
        addShortMovement(-1, 0);
        addShortMovement(0, -1);
        //diagonals
        addShortMovement(1, 1);
        addShortMovement(-1, 1);
        addShortMovement(-1, -1);
        addShortMovement(1, -1);
        //castling (TODO)
        //addShortMovement(0,3);
        //addShortMovement(0,-4);
    }

    public override void onMove()
    {
        //TODO: lose your ability to castle after first move
        //removeShortMovement(2, 0);
    }
}
