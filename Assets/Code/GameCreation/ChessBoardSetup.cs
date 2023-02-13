using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardSetup : MonoBehaviour
{
    public ChessTile[,] board;
    private GameObject gameObjectBoard;

    public Selection selector;
    public GameObject basePieces;
    public MapCreator mapCreator;
    public static GameObject whiteArmy, blackArmy;
    public static Material whiteColor, blackColor;
    public Material whiteCol, blackCol;
    public bool mapCreating = false;
    GameObject templatePawnWhite;
    GameObject templatePawnBlack;
    GameObject templateKnight;
    GameObject templateBishop;
    GameObject templateRook;
    GameObject templateQueen;
    GameObject templateKing;
    //Below used for map creation
    public Unit.Piece selectedPiece = Unit.Piece.PAWN;
    public Unit.Faction selectedFaction = Unit.Faction.WHITE;
    public ChessBoardSetup instance;

    // Start is called before the first frame update
    void Awake(){
        if(instance != null && instance != this){
            Debug.Log("Chess Board Already Exists, Deleting Default");
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
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

    public void DontDestroy(){
        DontDestroyOnLoad(whiteArmy);
        DontDestroyOnLoad(blackArmy);
    }

    //STANDARD setup, can change with different gamemodes.
    public void initializePieces()
    {
        //"armies" are just the empty container objects for each player       
        whiteArmy = new GameObject();
        blackArmy = new GameObject();
        whiteArmy.name = "whiteArmy";
        blackArmy.name = "blackArmy";
        whiteArmy.tag = "whiteArmy";
        blackArmy.tag = "blackArmy";

        //in setup phase, just copy a "template" of each piece.
        templatePawnWhite = basePieces.transform.GetChild(0).gameObject;
        templatePawnBlack = basePieces.transform.GetChild(1).gameObject;
        templateKnight = basePieces.transform.GetChild(2).gameObject;
        templateBishop = basePieces.transform.GetChild(3).gameObject;
        templateRook = basePieces.transform.GetChild(4).gameObject;
        templateQueen = basePieces.transform.GetChild(5).gameObject;
        templateKing = basePieces.transform.GetChild(6).gameObject;

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

    public void initializePieces(bool manual){

    }

    public void placePiece(int row, int col){ //Used for custom gamemodes       
        switch(selectedPiece)
        {
            case (Unit.Piece.PAWN):
                if(selectedFaction == Unit.Faction.WHITE){
                    board[row, col].occupant = GameObject.Instantiate(templatePawnWhite).AddComponent<Unit>();
                    board[row, col].occupant.SetUnit(selectedPiece, board[row, col], selectedFaction);                   
                }
                else{
                    board[row, col].occupant = GameObject.Instantiate(templatePawnBlack).AddComponent<Unit>();
                    board[row, col].occupant.SetUnit(selectedPiece, board[row, col], selectedFaction);                   
                }
                break;
            case (Unit.Piece.KNIGHT):
                board[row, col].occupant = GameObject.Instantiate(templateKnight).AddComponent<Unit>();
                board[row, col].occupant.SetUnit(Unit.Piece.KNIGHT, board[row, col], selectedFaction);
                break;
            case (Unit.Piece.BISHOP):
                board[row, col].occupant = GameObject.Instantiate(templateBishop).AddComponent<Unit>();
                board[row, col].occupant.SetUnit(Unit.Piece.BISHOP, board[row, col], selectedFaction);
                break;
            case (Unit.Piece.ROOK):
                board[row, col].occupant = GameObject.Instantiate(templateRook).AddComponent<Unit>();
                board[row, col].occupant.SetUnit(Unit.Piece.ROOK, board[row, col], selectedFaction);
                break;
            case (Unit.Piece.QUEEN):
                board[row, col].occupant = GameObject.Instantiate(templateQueen).AddComponent<Unit>();
                board[row, col].occupant.SetUnit(Unit.Piece.QUEEN, board[row, col], selectedFaction);
                break;
            case (Unit.Piece.KING):
                board[row, col].occupant = GameObject.Instantiate(templateKing).AddComponent<Unit>();
                board[row, col].occupant.SetUnit(Unit.Piece.KING, board[row, col], selectedFaction);
                break;
            case (Unit.Piece.DELETE): //Used to delete units
                if(board[row, col].occupant != null){
                    Destroy(board[row,col].occupant.gameObject);
                    board[row,col].occupant = null;
                }
                break;
            case (Unit.Piece.DEFAULT):
                //Do nothing
                break;
        }
    }

    public void restart(){
        if(whiteArmy != null){
            Destroy(whiteArmy);
        }
        if(blackArmy != null){
            Destroy(blackArmy);
        }
        initializePieces();
    }

    //used in movement scripts.
    //given a unit and destination tile, return 0 if new tile is empty, 1 if enemy unit, 2 if friendly unit, 3+ is special cases
    public int newTileRelation(Unit unit, ChessTile destination)
    {
        if(destination.occupant == null)
        {
            /*switch(unit.getPieceType()){
                case Unit.Piece.PAWN:
                    return 2;
                default:
                    return 0;
            }*/
            return 0;
        }
        if (unit.getFaction() != destination.occupant.getFaction())
        {
            if(destination.occupant.getPieceType() == Unit.Piece.KING){ //This piece, on its next turn, could kill the king, i.e. the king is in "check"
                return -1;
            }
            /*switch(unit.getPieceType()){
                case Unit.Piece.PAWN: //For pawns, pawn column - destination.colNum, if this is 0 or greater than pawn max striking distance, don't do anything (can't attack forward, 0)
                    if(unit.getCol() - destination.colNum != 0 && (unit.getCol() - destination.colNum < unit.GetComponent<MovingPawn>().strikingDistance || 
                   unit.getCol() - destination.colNum > -unit.GetComponent<MovingPawn>().strikingDistance)){
                          return 3;
                   }
                   else{
                          return 2; //Pretend its a friendly unit
                   }
                default:
                    return 1;
            }*/
            return 1;
            
        }
        /*else if(destination.occupant.getPieceType() == Unit.Piece.ROOK && unit.getPieceType() == Unit.Piece.KING){ //Same faction, if unit is king and destination is rook, attempt castling
            Debug.Log("Checking for movement");
            if(unit.GetComponent<MovingKing>().hasMoved == false && destination.occupant.GetComponent<MovingRook>().hasMoved == false){ //both valid for castling
                return 3;
            }
            else{
                return 2; //Invalid for one or both of them.
            }

        }*/

        else{
            //Debug.Log("dest " + destination.occupant.getPieceType());
            return 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
