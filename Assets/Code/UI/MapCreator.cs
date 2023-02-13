using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCreator : MonoBehaviour
{
    public ChessBoardSetup board;
    public int boardSize = 8;

    void Start()
    {
        board = GameObject.Find("chessBoard").GetComponent<ChessBoardSetup>();
        board.mapCreator = this;
    }

    public void updateMap(int row, int col){
        board.placePiece(row,col);
    }

    public void selectPiece(Unit.Piece piece){
        Debug.Log(piece);
        board.selectedPiece = piece;
    }

    public void selectFaction(Unit.Faction faction){
        Debug.Log(faction);
        board.selectedFaction = faction;
    }
}
