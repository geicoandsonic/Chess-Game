using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRook : GeneralMovement
{
    public bool hasMoved;
    void Awake()
    {
        moveSetup();
    }

    public override void moveSetup()
    {
        //cardinal
        addLongMovement(1, 0);
        addLongMovement(-1, 0);
        addLongMovement(0, 1);
        addLongMovement(0, -1);
        //castling (TODO)
        // For castling, need to check if next piece is rook. On rook, need to check if next piece is king. On either, check between the two if there are
        //pieces. Need to make sure both the rook and king have not moved, so maybe keep a track of both rooks and kings initial position
        addSpecialMovement(0,3);
        addSpecialMovement(0,-4);
    }

    public override void onMove()
    {
        hasMoved = true;
        listCleanup();
        //TODO: lose your ability to castle after first move
    }
}
