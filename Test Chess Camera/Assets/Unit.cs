using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum Piece { PAWN, KNIGHT, BISHOP, ROOK, QUEEN, KING }
    public enum Faction { WHITE, BLACK, REVOLUTION, UNDEAD }

    //fields to be changed
    private Piece pieceType;
    private int row, col;
    private Faction faction;
    public MovingPawn pawnUnit;

    public void SetUnit(Piece pieceT, int row, int col, Faction f)
    {
        pieceType = pieceT;
        this.row = row; this.col = col;
        faction = f;

        //gameObject.transform.parent = null;
        //set position
        float c = 0;
        gameObject.transform.position = new Vector3(1.5f * row, 0, -1.5f * col - c);

        //add to proper army
        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        if (faction == Faction.WHITE)
        {
            gameObject.transform.parent = ChessBoardSetup.whiteArmy.transform;
            foreach(MeshRenderer mr in mrs)
            {
                mr.material = ChessBoardSetup.whiteColor;
            }
        }
        if (faction == Faction.BLACK)
        {
            gameObject.transform.parent = ChessBoardSetup.blackArmy.transform;
            foreach (MeshRenderer mr in mrs)
            {
                mr.material = ChessBoardSetup.blackColor;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public string ToString()
    {
        return pieceType.ToString();
    }

    public Faction getFaction()
    {
        return faction;
    }

    public Piece getPieceType()
    {
        return pieceType;
    }

    public int getRow(){
        return row;
    }

    public int getCol(){
        return col;
    }

    public void displayMovementOptions(){ //Will eventually display ghost tiles of where player can go
        
    }

    public bool takeMove(int x, int y, GameObject tile){
        pawnUnit = gameObject.transform.GetChild(0).GetComponent<MovingPawn>();
        if(pawnUnit.attemptMovement(x,y,row,col)){
            row = tile.GetComponent<ChessTile>().row;
            col = tile.GetComponent<ChessTile>().colNum;
            Debug.Log("Unit col" + col);
            tile.GetComponent<ChessTile>().occupant = this;
            gameObject.transform.position = new Vector3(1.5f * row, 0, -1.5f * col);
            return true;
        }
        else{
            Debug.Log("Invalid Movement!");
        }
        return false;
    }
    
}
