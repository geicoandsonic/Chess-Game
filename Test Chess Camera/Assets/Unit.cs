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
    
    /*public Unit(Piece pieceT, GameObject physical, int row, int col, Faction f)
    {
        pieceType = pieceT;
        gameObj = physical;
        this.row = row; this.col = col;
        faction = f;

        gameObj.transform.parent = null;
        //set position
        gameObj.transform.position = new Vector3(1.5f * row, 0, -1.5f * col);

        //add to proper army
        if (faction == Faction.WHITE) gameObj.transform.parent = ChessBoardSetup.whiteArmy.transform;
        if (faction == Faction.BLACK) gameObj.transform.parent = ChessBoardSetup.blackArmy.transform;
    }*/

    public void SetUnit(Piece pieceT, int row, int col, Faction f)
    {
        pieceType = pieceT;
        this.row = row; this.col = col;
        faction = f;

        //gameObject.transform.parent = null;
        //set position
        float c = 0;  //1.5f * 7;
        gameObject.transform.position = new Vector3(1.5f * row, 0, -1.5f * col - c);

        //add to proper army
        if (faction == Faction.WHITE)
        {
            gameObject.transform.parent = ChessBoardSetup.whiteArmy.transform;
            GetComponentInChildren<MeshRenderer>().material = ChessBoardSetup.whiteColor;
        }
        if (faction == Faction.BLACK)
        {
            gameObject.transform.parent = ChessBoardSetup.blackArmy.transform;
            GetComponentInChildren<MeshRenderer>().material = ChessBoardSetup.blackColor;
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
    
}
