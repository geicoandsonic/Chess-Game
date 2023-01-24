using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessTile : MonoBehaviour
{
    public int row;
    public char column;
    private int colNum;
    public bool white;

    public Unit occupant;

    Material objMaterial;
    ChessBoardSetup board;
    Selection selector;

    void Start()
    {
        colNum = (int)column - 97;
        objMaterial = GetComponent<Renderer>().material;

        board = FindObjectOfType<ChessBoardSetup>();
        selector = FindObjectOfType<Selection>();
    }

    void OnMouseOver()
    {
        selector.setSelectorPosition(this);
    }
    void OnMouseExit()
    {
        selector.resetSelectorPosition();
    }

    void OnMouseDown()
    {
        Debug.Log("clicked " + getName());
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
