using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playerHasPiece;
    public Unit playerPiece;
    public bool playerOneTurn; //True if white chess side is playing i.e. player one, false if black chess side is playing i.e. player two.
    public ChessTile selectedTile;
    public enum GameState { DEFAULT,START,PLAYERONETURN,PLAYERTWOTURN,TIMEOUT,END}
    public GameState gameState;
    private GameState tempState;

    void Awake(){
        gameState = GameState.PLAYERONETURN; //Should be start on scene start, but for now its just playeroneturn
        playerHasPiece = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void changeGameState(){
        if(gameState == GameState.START){
            gameState = GameState.PLAYERONETURN;
        }
        else if(gameState == GameState.PLAYERONETURN){
            gameState = GameState.PLAYERTWOTURN;
        }
        else if(gameState == GameState.PLAYERTWOTURN){
            gameState = GameState.PLAYERONETURN;
        }
    }

    public void timeout(){
        tempState = gameState;
        gameState = GameState.TIMEOUT;
    }

    public void endTimeout(){
        gameState = tempState;
        tempState = GameState.DEFAULT;
    }

}
