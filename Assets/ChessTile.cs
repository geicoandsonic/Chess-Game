using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessTile : MonoBehaviour
{
    public int row;
    public char column;
    public int colNum;
    public bool white;

    public Unit occupant;
    private GameManager gameManager;
    Material objMaterial;
    ChessBoardSetup board;
    Selection selector;


    void Start()
    {
        //colNum = (int)column - 97;
        //Debug.Log("column " + (int)column);
        objMaterial = GetComponent<Renderer>().material;

        board = FindObjectOfType<ChessBoardSetup>();
        selector = FindObjectOfType<Selection>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    void OnMouseOver()
    {
        selector.setSelectorPosition(this);
    }
    void OnMouseExit()
    {
        selector.resetSelectorPosition();
    }

    // Check tile for what it is, if it has anything and give options on what to do.
    void OnMouseDown()
    {
        if(gameManager.gameState != GameManager.GameState.DEBUG_DELETE && gameManager.gameState != GameManager.GameState.DEBUG_MOVE){ //Basically if game state isnt in debug mode
            if (occupant != null){//We have found a piece
            selector.selectPiece(this);           
            }
            else{ //Tile is empty
                selector.emptySelection(this);
            }
        }
        else if(gameManager.gameState == GameManager.GameState.DEBUG_DELETE){
            Debug.Log("Delete debug path");
            if(occupant != null){       
                Destroy(occupant.gameObject);
                occupant = null;
            }
        }
        else if(gameManager.gameState == GameManager.GameState.DEBUG_MOVE){
            if(occupant == null){ //Clicked on empty tile, check if we can move something there
                if(gameManager.playerHasPiece){
                    gameManager.playerPiece.overrideMovement(this);
                    gameManager.playerPiece = null;
                    gameManager.playerHasPiece = false;
                }
                else{
                    Debug.Log("No piece to move");
                }
            }
            else{
                if(!gameManager.playerHasPiece) //No piece, just grabbing
                { // No piece selected to move yet
                    gameManager.playerHasPiece = true; //Now we have one
                    gameManager.playerPiece = occupant.GetComponent<Unit>();
                    gameManager.selectedTile = this;
                    selector.updateUI();
                }
                else{
                    gameManager.playerPiece.overrideMovement(this);
                    gameManager.playerPiece = null;
                    gameManager.playerHasPiece = false;
                }
            }
            
        }
    }

    





    public string getName()
    {
        return row.ToString() + column;
    }

    public string getOccupantName()
    {
        if (occupant != null)
        {            
            return occupant.getFaction() + " " + occupant.ToString();
        }
        else return "No one";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //returns the chessTile in "board" up and right by specified amounts
    public ChessTile getTileRelativeToMe(int upBy, int rightBy)
    {
        if (row + upBy >= 0 && row + upBy <= 7 && (colNum + rightBy >= 0 && colNum + rightBy <= 7))
        {
            return board.board[row + upBy, colNum + rightBy];
        }
        else
        {
            //Debug.Log("Couldn't find chess tile!");
            return null;
        }
    }
}
