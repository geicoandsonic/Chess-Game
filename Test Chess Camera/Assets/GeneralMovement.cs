using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GeneralMovement
{
    /*SHORT movement: dictates where you can move in limited capacity
     ex: pawn has [0,1] for moving UP 1 square (relative to their start) and 0 across.
     also starts with [0,2] for charge, but this gets removed after first move.
     can have [1,1] and/or [1,-1], or even [2,1] (en passant) if a piece can be attacked*/

    /*LONG movement: dictates where you can move "any amount" in
     ex: rooks have long movement in [1,0], [0,1], [0,-1], [-1,0]
     bishops have long movement in [1,1], [1,-1], [-1,1], [-1,-1]
     queens have all 8 of these.*/
    void moveSetup();

    void onMove();

    LinkedList<ChessTile> getPossibleMoves();

    void addLongMovement(int x, int y);

    void addShortMovement(int x, int y);

    void removeLongMovement(int x, int y);

    void removeShortMovement(int x, int y);
}
