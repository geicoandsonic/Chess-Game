using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingKing : GeneralMovement
{
    public bool hasMoved;
    void Awake()
    {
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
        // For castling, need to check if next piece is rook. On rook, need to check if next piece is king. On either, check between the two if there are
        //pieces. Need to make sure both the rook and king have not moved, so maybe keep a track of both rooks and kings initial position
        addSpecialMovement(0,2);
        addSpecialMovement(0,-2);
    }

    public override void onMove()
    {
        hasMoved = true;
        removeSpecialMovement(0,2);
        removeSpecialMovement(0,-2);
        listCleanup();
        //TODO: lose your ability to castle after first move
    }
}
