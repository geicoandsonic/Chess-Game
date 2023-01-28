using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI turnKeeper; //used to keep track of turns
    public TextMeshProUGUI turnCounter;
    private int turnCountInt = 1;

    public bool playerHasPiece;
    public Unit playerPiece;
    public bool playerOneTurn; //True if white chess side is playing i.e. player one, false if black chess side is playing i.e. player two.
    public ChessTile selectedTile;
    public enum GameState { DEFAULT,START,PLAYERONETURN,PLAYERTWOTURN,TIMEOUT,END}
    public GameState gameState;
    private GameState tempState;

    void Awake(){
        gameState = GameState.PLAYERONETURN; //Should be start on scene start, but for now its just playeroneturn
        turnKeeper.text = "WHITE to play"; //change this as well
        turnKeeper.color = Color.white;
        playerHasPiece = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void changeGameState(){
        if(gameState == GameState.START){
            gameState = GameState.PLAYERONETURN;
            turnKeeper.text = "WHITE to play";
            turnKeeper.color = Color.white;
        }
        else if(gameState == GameState.PLAYERONETURN){
            gameState = GameState.PLAYERTWOTURN;
            turnKeeper.text = "BLACK to play";
            turnKeeper.color = Color.gray;
        }
        else if(gameState == GameState.PLAYERTWOTURN){
            gameState = GameState.PLAYERONETURN;
            turnKeeper.text = "WHITE to play";
            turnKeeper.color = Color.white;
            //a "turn" ends when black makes its move
            turnCountInt++;
            turnCounter.text = "Turn #" + turnCountInt;
        }
    }

    public void timeout(){
        tempState = gameState;
        gameState = GameState.TIMEOUT;
        turnKeeper.text = "[!]  TIMEOUT  [!]";
        turnKeeper.color = Color.red;
    }

    public void endTimeout(){
        gameState = tempState;
        tempState = GameState.DEFAULT;
        turnKeeper.text = "[!]  DEFAULT  [!]";
        turnKeeper.color = Color.blue;
    }

}
