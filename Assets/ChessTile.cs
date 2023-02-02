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
        //Debug.Log("clicked " + getName());
        if (occupant != null){//We have found a piece
            selector.selectPiece(this);
            
        }
        else{ //Tile is empty
            selector.emptySelection(this);
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
