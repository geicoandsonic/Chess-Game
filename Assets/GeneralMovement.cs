using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMovement: MonoBehaviour
{
    /*SHORT movement: dictates where you can move in limited capacity
     ex: pawn has [0,1] for moving UP 1 square (relative to their start) and 0 across.
     also starts with [0,2] for charge, but this gets removed after first move.
     can have [1,1] and/or [1,-1], or even [2,1] (en passant) if a piece can be attacked*/

    /*LONG movement: dictates where you can move "any amount" in
     ex: rooks have long movement in [1,0], [0,1], [0,-1], [-1,0]
     bishops have long movement in [1,1], [1,-1], [-1,1], [-1,-1]
     queens have all 8 of these.*/
    public LinkedList<(int x, int y)> shortMovements = new LinkedList<(int x, int y)>();
    public LinkedList<(int x, int y)> longMovements = new LinkedList<(int x, int y)>();
    public LinkedList<(int x, int y)> specialMovements = new LinkedList<(int x, int y)>();
    //[SerializeField] private GameObject ghostTile;
    [SerializeField] private Unit unit;
    private ChessBoardSetup board;

    //private LinkedList<GameObject> ghostTileList = new LinkedList<GameObject>();
    private LinkedList<ChessTile> chessTileList = new LinkedList<ChessTile>();

    public virtual void moveSetup()
    {
        //override and add "default" movements of the piece here.
    }

    public virtual void onMove()
    {
        //override with what this piece does, if anything, when it moves.
    }


    //add to chess tile list (list of chessTiles for your perusal)
    protected void addTileToLists(int upBy, int rightBy)
    {
        if(upBy >= 0 && upBy <= 7 && rightBy >= 0 && rightBy <= 7){
            chessTileList.AddFirst(board.board[upBy,rightBy]);
        }
        
    }

    //clear the chess tile list
    protected void listCleanup()
    {

        chessTileList.Clear();
    }

    //get all possible tiles this piece could move to next turn.
    public LinkedList<ChessTile> getPossibleMoves()
    {
        unit = GetComponent<Unit>();
        if(board == null) { board = FindObjectOfType<ChessBoardSetup>(); }
        listCleanup();
        foreach (var move in shortMovements)
        {
            addTileToLists(unit.getRow() + move.x, unit.getCol() + move.y);
        }
        //thoughtful design: are things getting added twice? please check...
        foreach (var move in longMovements)
        {
            for (int i = 1; i <= 8; i++)
            {
                int tempRow = unit.getRow() + move.x * i;
                int tempCol = unit.getCol() + move.y * i;
                //check 2 things: the row/col are in bounds, and the path here isn't blocked
                if (tempRow >= 0 && tempRow <= 7 && tempCol >= 0 && tempCol <= 7)
                {

                    int code = board.newTileRelation(unit, board.board[tempRow, tempCol]);
                    //empty tile. can move here and past it
                    if(code == 0)
                    {
                        addTileToLists(tempRow, tempCol);
                    } else if(code == 1) //enemy tile. can move here but not past it
                    {
                        addTileToLists(tempRow, tempCol);
                        break;
                    } else if(code == 2) //friendly tile. can't move here
                    {
                        break;
                    }
                    
                }
                else break;
                
                //if(board[tempRow, tempCol])
            }
        }
        return chessTileList;
        
    }

    public LinkedList<ChessTile> getPossibleMoves(bool enableSpecial)
    {
        unit = GetComponent<Unit>();
        if(board == null) { board = FindObjectOfType<ChessBoardSetup>(); }
        listCleanup();
        foreach (var move in shortMovements)
        {
            //this is adding extra movement for pawn so the middle piece reports as an enemy.
            if(unit.getPieceType() == Unit.Piece.PAWN){
                if(board.board[unit.getRow() + move.x, unit.getCol() + move.y].occupant == null){
                    addTileToLists(unit.getRow() + move.x, unit.getCol() + move.y);
                }
            }
            else{
                addTileToLists(unit.getRow() + move.x, unit.getCol() + move.y);
            }           
        }
        //thoughtful design: are things getting added twice? please check...
        foreach (var move in longMovements)
        {
            for (int i = 1; i <= 8; i++)
            {
                int tempRow = unit.getRow() + move.x * i;
                int tempCol = unit.getCol() + move.y * i;
                //check 2 things: the row/col are in bounds, and the path here isn't blocked
                if (tempRow >= 0 && tempRow <= 7 && tempCol >= 0 && tempCol <= 7)
                {

                    int code = board.newTileRelation(unit, board.board[tempRow, tempCol]);
                    //empty tile. can move here and past it
                    if(code == 0)
                    {
                        addTileToLists(tempRow, tempCol);
                    } else if(code == 1) //enemy tile. can move here but not past it
                    {
                        addTileToLists(tempRow, tempCol);
                        break;
                    } else if(code == 2) //friendly tile. can't move here
                    {
                        break;
                    }
                    
                }
                else break;
                
                //if(board[tempRow, tempCol])
            }
        }
        if(enableSpecial){
            switch(unit.getPieceType()){
                case Unit.Piece.PAWN:
                    Debug.Log("In switch for pawn in general");
                    foreach (var move in specialMovements){
                        //for(int i = 0; i < 1; i++){
                            int tempRow = unit.getRow() + move.x;
                            int tempCol = unit.getCol() + move.y; //Start at unit col over to the right one, the middle, then left
                            Debug.Log("Current col check: " + tempCol);
                            Debug.Log("unit column: " + unit.getCol());
                            //check 2 things: the row/col are in bounds, and the path here isn't blocked
                            if (tempRow >= 0 && tempRow <= 7 && tempCol >= 0 && tempCol <= 7)
                            {
                                
                                int code = board.newTileRelation(unit, board.board[tempRow, tempCol]);
                                //empty tile. can move here and past it
                                if(code == 0)
                                {
                                    addTileToLists(tempRow, tempCol);
                                    Debug.Log("0");
                                } else if(code == 1) //enemy tile. can move here but not past it, probably shouldn't happen for a pawn
                                {
                                Debug.Log("1");
                                    addTileToLists(tempRow, tempCol);
                                } else if(code == 2) //friendly tile. can't move here
                                {
                                Debug.Log("2");
                                }
                                else if(code == 3) // special case, can attack
                                {
                                Debug.Log("3");
                                    addTileToLists(tempRow, tempCol);
                                }
                    
                             }
                            else break;
                         //}
                    }
                    break;
                default:
                break;   
                }
        }
        return chessTileList;
    }


    //move the tile to an empty destination
    public bool attemptMove2(ChessTile destination)
    {
        if(unit.getPieceType() == Unit.Piece.PAWN){ //Pawn has special cases, shouldn't add moves that won't normally work (diagonals)
            if(destination.occupant == null){ //Pawn is not going to another unit, so it can only move forward, same as the else if below
                if (validMove(getPossibleMoves(), destination))
                {
                    onMove();
                    Debug.Log("new move method worked, path found for a pawn");
                    return true;
                 }
                 return false;
            }
            else{ //Pawn is seeing another unit (HAS NOT DIFFERENTIATED WHITE OR BLACK YET)
                if (validMove(getPossibleMoves(true), destination))
                {
                    onMove();
                    Debug.Log("new move method worked, path found for a pawn, special move");
                    return true;
                 }
                 return false;
            }
        }
        else if (validMove(getPossibleMoves(), destination))
        {
            onMove();
            Debug.Log("new move method worked, path found");
            return true;
        }
        return false;
    }

    //basically a rewrite of the "contains" method.
    private bool validMove(LinkedList<ChessTile> possible, ChessTile dest)
    {
        foreach(ChessTile possibleMove in possible){
            Debug.Log(dest.getName());
            if (possibleMove.getName().Equals(dest.getName())) return true;
        }
        return false;
    }

    public void addShortMovement(int x, int y)
    {
        //no duplicates!
        if (!shortMovements.Contains((x, y)))
        {
            shortMovements.AddFirst((x, y));
        }
    }

    public void removeShortMovement(int x, int y)
    {
        shortMovements.Remove((x, y));
    }

    public void addLongMovement(int x, int y)
    {
        //no duplicates!
        if (!longMovements.Contains((x, y)))
        {
            longMovements.AddFirst((x, y));
        }
    }

    public void removeLongMovement(int x, int y)
    {
        longMovements.Remove((x, y));
    }

    public void addSpecialMovement(int x, int y){
        //no duplicates!
        if (!specialMovements.Contains((x, y)))
        {
            specialMovements.AddFirst((x, y));
        }
    }
}
