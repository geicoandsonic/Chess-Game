using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingKing : GeneralMovement
{
    public bool hasMoved;
    public bool isInCheck;
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
        addSpecialMovement(0,2, Validify_ShortCastle);
        //addSpecialMovement(0,-2, Validify_Castle);
    }

    public override void onMove()
    {
        hasMoved = true;
        removeSpecialMovement(0,2, Validify_ShortCastle);
        //removeSpecialMovement(0,-2, Validify_ShortCastle);
        listCleanup();
        //TODO: lose your ability to castle after first move
    }

    //special movement - "castling"
    private bool Validify_ShortCastle()
    {
        /*the conditions for a castle are as follows:
         * 1. neither the king nor the rook involved has moved (this condition is checked outside of this method)
         * 2. no pieces are in between the king and rook
         * 3. the king has not been put into check (this condition is checked outside of this method)
         * 
         * so in this method we're really just checking condition 2.
        */
        //short castling always goes to the right.
        ChessTile intermediate1 = unit.getLocation().getTileRelativeToMe(0, 1);
        ChessTile intermediate2 = unit.getLocation().getTileRelativeToMe(0, 2);
        ChessTile castleSpot = unit.getLocation().getTileRelativeToMe(0, 3);

        if ((intermediate1 != null && board.newTileRelation(unit, intermediate1) == 0) &&
           (intermediate2 != null && board.newTileRelation(unit, intermediate2) == 0))
        {
            //also check if the castle hasnt moved?
            if(castleSpot.occupant != null && castleSpot.getOccupantName().Equals("WHITE ROOK"))
            {
                return true;
            }
            
        }
        return false;
    }

    //moves the rook to its proper spot in castling as well.
    private void Action_ShortCastle()
    {
        //ChessTile castleSpot = unit.getLocation().getTileRelativeToMe(0, 3);
        //castleSpot.occupant
    }

    private bool Validify_LongCastle()
    {
        ChessTile intermediate1 = unit.getLocation().getTileRelativeToMe(0, -1);
        ChessTile intermediate2 = unit.getLocation().getTileRelativeToMe(0, -2);
        ChessTile intermediate3 = unit.getLocation().getTileRelativeToMe(0, -3);

        if ((intermediate1 != null && board.newTileRelation(unit, intermediate1) == 0) &&
           (intermediate2 != null && board.newTileRelation(unit, intermediate2) == 0) &&
           (intermediate3 != null && board.newTileRelation(unit, intermediate3) == 0))
        {
            return true;
        }
        return false;
    }
}
