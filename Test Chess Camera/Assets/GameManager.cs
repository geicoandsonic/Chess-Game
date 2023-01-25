using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playerOneHasPiece;
    public Unit playerOnePiece;
    public bool playerOneTurn; //True if white chess side is playing i.e. player one, false if black chess side is playing i.e. player two.
    public ChessTile selectedTile;

    void Awake(){
        playerOneHasPiece = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public enum gameState
    {
        DEFAULT,
        START,
        PLAYERONETURN,
        PLAYERTWOTURN,
        TIMEOUT,
        END
    }
}
