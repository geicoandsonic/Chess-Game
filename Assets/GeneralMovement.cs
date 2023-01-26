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

    protected void addTileToLists(int upBy, int rightBy)
    {

        //add to chess tile list (list of chessTiles for your perusal)
        if(upBy >= 0 && upBy <= 7 && rightBy >= 0 && rightBy <= 7){
            chessTileList.AddFirst(board.board[upBy,rightBy]);
        }
        
    }

    protected void listCleanup()
    {

        chessTileList.Clear();
    }

    public LinkedList<ChessTile> getPossibleMoves()
    {
        unit = GetComponent<Unit>();
        if(board == null) { board = FindObjectOfType<ChessBoardSetup>(); }
        listCleanup();
        foreach (var move in shortMovements)
        {
            addTileToLists(unit.getRow() + move.x, unit.getCol() + move.y);
        }
        return chessTileList;
        
    }

    /*public bool attemptMovement(int row, int col, int currRow, int currCol)
    {
        Debug.Log("Attempting movement");
        foreach (var move in shortMovements)
        {
            //Debug.Log("move.x = " + move.x);
            //Debug.Log("currRow and move.x = " + currRow + " " + move.x);
            //Debug.Log("target row = " + row);
            if (currRow + move.x == row)
            {
                //Debug.Log("move.y = " + move.y);
                //Debug.Log("currCol and move.y = " + currCol + " " + move.y);
                //Debug.Log("target col = " + col);
                if (currCol + move.y == col)
                {
                    onMove();
                    Debug.Log("Successfully found path");
                    return true;
                }
            }
        }

        //DO LATER
        foreach (var move in longMovements)
        {

        }
        return false;
    }*/

    public bool attemptMove2(ChessTile destination)
    {
        if (validMove(getPossibleMoves(), destination))
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
}
