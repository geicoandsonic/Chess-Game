using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardSetup : MonoBehaviour
{
    public ChessTile[,] board;
    private GameObject gameObjectBoard;

    public GameObject basePieces;
    public static GameObject whiteArmy, blackArmy;
    public static Material whiteColor, blackColor;
    public Material whiteCol, blackCol;

    // Start is called before the first frame update
    void Start()
    {
        whiteColor = whiteCol;
        blackColor = blackCol;

        gameObjectBoard = gameObject;

        board = new ChessTile[8,8];
        for(int i=0; i<8; i++)
        {
            GameObject row = gameObjectBoard.transform.GetChild(i).gameObject;
            for (int j=0; j<8; j++)
            {
                GameObject currTile = row.transform.GetChild(j).gameObject;
                ChessTile chessTile = currTile.GetComponent<ChessTile>();

                //set fields
                if (i % 2 == j % 2)
                    chessTile.white = true;
                else chessTile.white = false;

                chessTile.row = i;
                chessTile.column = (char)(97 + j);
                chessTile.colNum = j;

                //set board
                board[i, j] = chessTile;
            }
        }

        initializePieces();
    }



    //STANDARD setup, can change with different gamemodes.
    void initializePieces()
    {
        //"armies" are just the empty container objects for each player       
        whiteArmy = new GameObject();
        blackArmy = new GameObject();
        whiteArmy.name = "whiteArmy";
        blackArmy.name = "blackArmy";

        //in setup phase, just copy a "template" of each piece.
        GameObject templatePawnWhite = basePieces.transform.GetChild(0).gameObject;
        GameObject templatePawnBlack = basePieces.transform.GetChild(1).gameObject;
        GameObject templateKnight = basePieces.transform.GetChild(2).gameObject;
        GameObject templateBishop = basePieces.transform.GetChild(3).gameObject;
        GameObject templateRook = basePieces.transform.GetChild(4).gameObject;
        GameObject templateQueen = basePieces.transform.GetChild(5).gameObject;
        GameObject templateKing = basePieces.transform.GetChild(6).gameObject;

        //instantiate pawns
        for (int j=0; j<8; j++)
        {
            board[1, j].occupant = GameObject.Instantiate(templatePawnWhite).AddComponent<Unit>();
            board[1, j].occupant.SetUnit(Unit.Piece.PAWN, board[1, j], Unit.Faction.WHITE);
            board[6, j].occupant = GameObject.Instantiate(templatePawnBlack).AddComponent<Unit>();
            board[6, j].occupant.SetUnit(Unit.Piece.PAWN, board[6, j], Unit.Faction.BLACK);
        }


        //instantiate other pieces
        Unit.Faction f = Unit.Faction.WHITE;
        for (int i=0; i < 8; i += 7)
        {
            if(i > 0) { f = Unit.Faction.BLACK; }

            board[i, 0].occupant = GameObject.Instantiate(templateRook).AddComponent<Unit>();
            board[i, 0].occupant.SetUnit(Unit.Piece.ROOK, board[i, 0], f);
            board[i, 7].occupant = GameObject.Instantiate(templateRook).AddComponent<Unit>();
            board[i, 7].occupant.SetUnit(Unit.Piece.ROOK, board[i, 7], f);

            board[i, 1].occupant = GameObject.Instantiate(templateKnight).AddComponent<Unit>();
            board[i, 1].occupant.SetUnit(Unit.Piece.KNIGHT, board[i, 1], f);
            board[i, 6].occupant = GameObject.Instantiate(templateKnight).AddComponent<Unit>();
            board[i, 6].occupant.SetUnit(Unit.Piece.KNIGHT, board[i, 6], f);

            board[i, 2].occupant = GameObject.Instantiate(templateBishop).AddComponent<Unit>();
            board[i, 2].occupant.SetUnit(Unit.Piece.BISHOP, board[i, 2], f);
            board[i, 5].occupant = GameObject.Instantiate(templateBishop).AddComponent<Unit>();
            board[i, 5].occupant.SetUnit(Unit.Piece.BISHOP, board[i, 5], f);
        }

        //instantiate the royals
        board[0, 3].occupant = GameObject.Instantiate(templateQueen).AddComponent<Unit>();
        board[0, 3].occupant.SetUnit(Unit.Piece.QUEEN, board[0, 3], Unit.Faction.WHITE);
        board[0, 4].occupant = GameObject.Instantiate(templateKing).AddComponent<Unit>();
        board[0, 4].occupant.SetUnit(Unit.Piece.KING, board[0, 4], Unit.Faction.WHITE);

        board[7, 3].occupant = GameObject.Instantiate(templateQueen).AddComponent<Unit>();
        board[7, 3].occupant.SetUnit(Unit.Piece.QUEEN, board[7, 3], Unit.Faction.BLACK);
        board[7, 4].occupant = GameObject.Instantiate(templateKing).AddComponent<Unit>();
        board[7, 4].occupant.SetUnit(Unit.Piece.KING, board[7, 4], Unit.Faction.BLACK);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
