using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPawn : GeneralMovement
{

    //public LinkedList<(int x, int y)> shortMovements = new LinkedList<(int x, int y)>();
    //[SerializeField] private GameObject ghostTile;

    void Awake(){
        //ghostTile = GameObject.FindWithTag("ghostTile");
        moveSetup();
    }

    new public void moveSetup()
    {
        shortMovements.AddFirst((1,0));
        shortMovements.AddFirst((2,0));
    }

    new public void onMove()
    {
        shortMovements.Remove((2,0));
    }
}
