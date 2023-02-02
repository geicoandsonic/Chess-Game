using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPawn : GeneralMovement
{
    [SerializeField] private bool isWhitePawn;
    [SerializeField] private bool justMoved2; //Boolean to check if the pawn moved 2 squares 
    public int strikingDistance = 1;
    void Awake(){
        moveSetup();
    }

    public override void moveSetup()
    {
        if(isWhitePawn){
            addShortMovement(1, 0);
            addSpecialMovement(2, 0, Validify_Charge);
            addSpecialMovement(1,1, Validify_PawnAttackRight);
            addSpecialMovement(1,-1, Validify_PawnAttackLeft);
            //addShortMovement(2,1); //Pawn has not moved yet, and enemy unit is in range
            //addShortMovement(2,-1); //not sure this is allowed tbh
            //add E.P here later, probably under a "special cases". We need to track the enemy's last turn movement.
        }
        else{
            addShortMovement(-1, 0);
            addSpecialMovement(-2, 0, Validify_Charge);
            addSpecialMovement(-1,1, Validify_PawnAttackRight);
            addSpecialMovement(-1,-1, Validify_PawnAttackLeft);
            //addShortMovement(-2,1); //Pawn has not moved yet, and enemy unit is in range
            //addShortMovement(-2,-1);
        }
    }

    public override void onMove()
    {
        if(isWhitePawn){
            removeSpecialMovement(2, 0, Validify_Charge);
        }
        else{
            removeSpecialMovement(-2, 0, Validify_Charge);
        }

        listCleanup();
    }

    //special movement - pawn can jump forward 2 spaces on its first move, if nothing blocks it.
    private bool Validify_Charge()
    {
        //valid if the 2 spaces directly in front of the pawn are unoccupied
        //(also only valid on first move, but onMove solves that problem already)
        int amnt = 1;
        if (!isWhitePawn) amnt = -1;

        ChessTile dest1 = unit.getLocation().getTileRelativeToMe(amnt, 0);
        ChessTile dest2 = unit.getLocation().getTileRelativeToMe(amnt*2, 0);
        if ((dest1 != null && board.newTileRelation(unit, dest1) == 0) &&
           (dest2 != null && board.newTileRelation(unit, dest2) == 0))
        {
            justMoved2 = true;
            return true;
        }
        return false;
    }

    //used for en passant rules
    /*private void Action_Charge()
    {

    }*/

    //special movement - pawn can only attack diagonally 1 space ahead
    private bool Validify_PawnAttackLeft()
    {
        //valid only if there's a piece to attack diagonally.
        int amnt = 1;
        if (!isWhitePawn) amnt = -1;

        ChessTile dest = unit.getLocation().getTileRelativeToMe(amnt, -1);
        if (dest != null && board.newTileRelation(unit, dest) == 1)
        {
            return true;
        }
        else if(dest != null){
            if(dest.occupant != null){
                if(dest.occupant.getPieceType() == Unit.Piece.KING){
                    if(board.newTileRelation(unit, dest) == -1){
                        return true;
                    }
                }
            }
            
        }
        return false;
    }


    //same as above but other direction. cant include both in one method, unfortunately, or else 
    //when one attack is available both attacks are always available
    private bool Validify_PawnAttackRight()
    {
        //valid only if there's a piece to attack diagonally.
        int amnt = 1;
        if (!isWhitePawn) amnt = -1;

        ChessTile dest = unit.getLocation().getTileRelativeToMe(amnt, 1);
        if (dest != null && board.newTileRelation(unit, dest) == 1)
        {
            return true;
        }
        else if(dest != null){
            if(dest.occupant != null){
                if(dest.occupant.getPieceType() == Unit.Piece.KING){
                    if(board.newTileRelation(unit, dest) == -1){
                        return true;
                    }
                }
            }
            
        }
        return false;
    }
}
