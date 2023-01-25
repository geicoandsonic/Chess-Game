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
    [SerializeField] private GameObject ghostTile;
    private Unit unit;

    void Awake()
    {
        ghostTile = GameObject.FindWithTag("ghostTile");
        unit = gameObject.GetComponent<Unit>();
        moveSetup();
    }

    public void moveSetup()
    {
        //override and add "default" movements of the piece here.
    }

    public void onMove()
    {
        //override with what this piece does, if anything, when it moves.
    }

    public LinkedList<ChessTile> getPossibleMoves()
    {
        foreach (var move in shortMovements)
        {
            //Do this later
        }
        throw new System.NotImplementedException();
    }

    public bool attemptMovement(int row, int col, int currRow, int currCol)
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
