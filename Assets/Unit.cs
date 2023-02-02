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
    public Unit storeUnit; //Unit that is stored when checking if king is in check
    public ChessTile prevLoc; //Chess tile stored for checking if king is in check;
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

    public void fakeMovement(ChessTile destination){ //This fakes unit movement, which removes the unit from its current tile (now units can pass through), stores then kills the Unit
    //on the destination tile (if there is one), and calculates if this movement will still put the king in check. If this is the case, the movement is illegal, and therefore
    //will not be a move the player can make. Otherwise, if it is valid, the player can make if it they want. Either way, after this query has been made, the everything must be reset
    //to before the player takes a move.
        if(destination.occupant != null){ //Destination tile has a piece, so we must store it for later
            Debug.Log("Destination occupant not null, storing");
            storeUnit = destination.occupant;
            destination.occupant = null; //Effectively clear its reference from the tile to move our unit there
        }
        prevLoc = location; //Storing the chesstile we just came from
        Debug.Log("prevLoc " + prevLoc);
        location.occupant = null; //Clearing the chesstile we are on of their occupant (i.e. our unit we are moving)
        location = destination;
        Debug.Log("New location " + location);
        location.occupant = this; //We have now switched to the destination chesstile, and made our unit its occupant
        Debug.Log("New occupant " + location.occupant);
    }

    public void resetPosition(){ //Called to reset our unit to its original position (i.e. before we take a move)
        location.occupant = null; //Remove current location's occupant
        if(storeUnit != null){
            Debug.Log("Reseting stored unit");
            location.occupant = storeUnit; // ChessTile we moved to now has its unit back
            storeUnit = null; //reset for later
        }       
        location = prevLoc; //Moving back to original tile
        Debug.Log("Reseting location " + location);
        prevLoc = null;
        location.occupant = this; //Our original tile now has this unit.
        Debug.Log("Location occupant after reset " + location.occupant);
    }
    
}
