using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPawn : MonoBehaviour
{

    public LinkedList<(int x, int y)> shortMovements = new LinkedList<(int x, int y)>();
    [SerializeField] private GameObject ghostTile;

    void Awake(){
        ghostTile = GameObject.FindWithTag("ghostTile");
        moveSetup();
    }

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
        foreach(var move in shortMovements){
            //Do this later
        }
        throw new System.NotImplementedException();
    }

    public bool attemptMovement(int row, int col, int currRow, int currCol){
        Debug.Log("Attempting movement");
        foreach(var move in shortMovements){
            Debug.Log("move.x = " + move.x);
            Debug.Log("currRow and move.x = " + currRow + " " +  move.x);
            Debug.Log("target row = " + row);
            if(currRow + move.x == row){
                Debug.Log("move.y = " + move.y);
                Debug.Log("currCol and move.y = " + currCol + " " +  move.y);
                Debug.Log("target col = " + col);
                if(currCol + move.y == col){
                    onMove();
                    Debug.Log("Successfully found path");
                    return true;
                }
            }
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
