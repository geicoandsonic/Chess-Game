using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPawn : GeneralMovement
{

    //new private LinkedList<(int x, int y)> shortMovements = new LinkedList<(int x, int y)>();
    //[SerializeField] private GameObject ghostTile;

    void Awake(){
        //ghostTile = GameObject.FindWithTag("ghostTile");
        moveSetup();
    }

    public override void moveSetup()
    {
        addShortMovement(1, 0);
        addShortMovement(2, 0);
        //shortMovements.AddFirst((1,0));
        //shortMovements.AddFirst((2,0));
    }

    public override void onMove()
    {
        removeShortMovement(2, 0);
        //shortMovements.Remove((2,0));
    }
}
