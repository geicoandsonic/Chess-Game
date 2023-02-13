using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whatPiece : MonoBehaviour
{
    public MapCreator map;
    [SerializeField] private enum Piece {PAWN,KNIGHT,BISHOP,ROOK,QUEEN,KING,DELETE,DEFAULT}
    [SerializeField] private Piece pieceType;
    void Awake(){
        map = GameObject.Find("ChessTilesPanel").GetComponent<MapCreator>();
    }

    public void sendPiece(){
        switch(pieceType)
        {
            case (Piece.PAWN):
                map.selectPiece(Unit.Piece.PAWN);
                break;
            case (Piece.KNIGHT):
                map.selectPiece(Unit.Piece.KNIGHT);
                break;
            case (Piece.BISHOP):
                map.selectPiece(Unit.Piece.BISHOP);
                break;
            case (Piece.ROOK):
                map.selectPiece(Unit.Piece.ROOK);
                break;
            case (Piece.QUEEN):
                map.selectPiece(Unit.Piece.QUEEN);
                break;
            case (Piece.KING):
                map.selectPiece(Unit.Piece.KING);
                break;
            case (Piece.DELETE):
                map.selectPiece(Unit.Piece.DELETE); //Unit has no enum DELETE, but does have DEFAULT
                break;
            default:
                map.selectPiece(Unit.Piece.DEFAULT); //Should do nothing, since we may hit a square accidentally opening another menu
                break;
        }
    }
}
