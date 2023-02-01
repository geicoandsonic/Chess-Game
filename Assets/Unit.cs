using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum Piece { PAWN, KNIGHT, BISHOP, ROOK, QUEEN, KING }
    public enum Faction { WHITE, BLACK, REVOLUTION, UNDEAD }

    //fields to be changed
    private Piece pieceType;
    private ChessTile location;
    //private int row, col;
    private Faction faction;
    public GeneralMovement movement;

    public void SetUnit(Piece pieceT, ChessTile tile, Faction f)
    {
        pieceType = pieceT;
        location = tile;
        faction = f;

        //gameObject.transform.parent = null;
        //set position
        gameObject.transform.position = new Vector3(1.5f * location.row, 0, -1.5f * location.colNum);

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
        movement = GetComponent<GeneralMovement>();
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

    public void setPiece(Unit.Piece pieceType){
        this.pieceType = pieceType;
        getPieceType();
    }

    public string getFactionString(){
        if(faction == Faction.WHITE){
            return "white";
        }
        else{
            return "black";
        }
    }

    public Piece getPieceType()
    {
        return pieceType;
    }

    public ChessTile getLocation()
    {
        return location;
    }

    public int getRow(){
        return location.row;
    }

    public int getCol(){
        return location.colNum;
    }

    /*public bool takeMove(ChessTile destination){
        movement = gameObject.GetComponent<GeneralMovement>();

        if(movement.attemptMove2(destination.GetComponent<ChessTile>()))
        {
            if(destination.occupant != null){
                Destroy(destination.occupant.gameObject);
                destination.occupant = null;
            }
            location.occupant = null;
            location = destination;
            //Debug.Log("Unit col" + location.colNum);
            location.occupant = this;
            gameObject.transform.position = new Vector3(1.5f * location.row, 0, -1.5f * location.colNum);
            return true;
        }
        else{
            Debug.Log("Invalid Movement!");
        }
        return false;
    }*/

    public void overrideMovement(ChessTile destination){
        if(destination.occupant != null){
            Destroy(destination.occupant.gameObject);
            destination.occupant = null;
        }
        location.occupant = null;
        location = destination;
        location.occupant = this;
        gameObject.transform.position = new Vector3(1.5f * location.row, 0, -1.5f * location.colNum);
    }
    
}
