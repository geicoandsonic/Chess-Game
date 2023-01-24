using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playerOneHasPiece;
    public Unit playerOnePiece;

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
