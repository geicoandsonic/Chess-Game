using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPawn : GeneralMovement
{

    private LinkedList<(int, int)> shortMovements;

    public void moveSetup()
    {
        shortMovements.AddFirst((1,0));
        shortMovements.AddFirst((2,0));
    }

    public void onMove()
    {
        shortMovements.Remove((2,0));
    }

    public LinkedList<ChessTile> getPossibleMoves()
    {
        //for(int i=0; i<shortMovements.)
        throw new System.NotImplementedException();
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



    //pawns don't make long moves
    public void addLongMovement(int x, int y)
    {
        throw new System.NotImplementedException();
    }

    public void removeLongMovement(int x, int y)
    {
        throw new System.NotImplementedException();
    }
}
