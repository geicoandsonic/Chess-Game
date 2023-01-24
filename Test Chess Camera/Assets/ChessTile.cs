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
        Debug.Log("clicked " + getName());
        if (occupant != null){//We have found a piece
            //if(occupant.getFaction() == Player Faction) will be used for checking if you have selected your own pieces
            if(!gameManager.playerOneHasPiece){ // Player One currently does not have a piece selected
                gameManager.playerOneHasPiece = true; //Now they do
                gameManager.playerOnePiece = occupant.GetComponent<Unit>();
                gameManager.tile = this;
            }
            else{ // Player One has a piece already, so we need to switch them
                Debug.Log("Switching pieces " + getName());
                 gameManager.playerOnePiece = occupant.GetComponent<Unit>();   
                 gameManager.tile = this;
            }
            
        }
        else{ //Tile is empty
            if(gameManager.playerOneHasPiece){
                Debug.Log("Taking move " + getName());
                if(gameManager.playerOnePiece.takeMove(row,colNum, gameObject)){
                    gameManager.tile.occupant = null;
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
}
