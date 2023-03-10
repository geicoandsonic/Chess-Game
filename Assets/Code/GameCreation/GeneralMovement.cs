using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMovement: MonoBehaviour
{
    /*SHORT movement: dictates where you can move in limited capacity
     ex: pawn has [0,1] for moving UP 1 square (relative to their start) and 0 across.
     also starts with [0,2] for charge, but this gets removed after first move.
     can have [1,1] and/or [1,-1], or even [2,1] (en passant) if a piece can be attacked*/

    /*LONG movement: dictates where you can move "any amount" in
     ex: rooks have long movement in [1,0], [0,1], [0,-1], [-1,0]
     bishops have long movement in [1,1], [1,-1], [-1,1], [-1,-1]
     queens have all 8 of these.
     
     SPECIAL movement: movement is only valid if certain conditions are met (defined
     by the boolean method passed in with this movement.)*/
    public delegate bool Validity_Checker();
    public delegate void Special_Action();

    public LinkedList<(int x, int y)> shortMovements = new LinkedList<(int x, int y)>();
    public LinkedList<(int x, int y)> longMovements = new LinkedList<(int x, int y)>();
    public LinkedList<(int x, int y, Validity_Checker vc, Special_Action action)> specialMovements = 
        new LinkedList<(int x, int y, Validity_Checker vc, Special_Action action)>();
    //[SerializeField] private GameObject ghostTile;
    [SerializeField] protected Unit unit;
    protected ChessBoardSetup board;

    //private LinkedList<GameObject> ghostTileList = new LinkedList<GameObject>();
    private LinkedList<(ChessTile tile, int tileType, Special_Action action)> chessTileList = 
        new LinkedList<(ChessTile tile, int tileType, Special_Action action)>();

    public virtual void moveSetup()
    {
        //override and add "default" movements of the piece here.
    }

    public virtual void onMove()
    {
        //override with what this piece does, if anything, when it moves.
    }

    void Start()
    {
        unit = GetComponent<Unit>();
        board = FindObjectOfType<ChessBoardSetup>();
    }


    //add to chess tile list (list of chessTiles for your perusal)
    protected void addTileToLists(int upBy, int rightBy, int tileType, Special_Action action)
    {
        if(upBy >= 0 && upBy <= 7 && rightBy >= 0 && rightBy <= 7){
            chessTileList.AddFirst((board.board[upBy,rightBy], tileType, action));
        }
        
    }

    //clear the chess tile list
    protected void listCleanup()
    {

        chessTileList.Clear();
    }

    public LinkedList<(ChessTile tile, int tileType, Special_Action action)> getPossibleMoves(bool enableSpecial)
    {
        listCleanup();
        foreach (var move in shortMovements)
        {
            //this is adding extra movement for pawn so the middle piece reports as an enemy.
            if(unit.getPieceType() == Unit.Piece.PAWN){
                if(board.board[unit.getRow() + move.x, unit.getCol() + move.y].occupant == null){
                    addTileToLists(unit.getRow() + move.x, unit.getCol() + move.y, 0, null);
                }
            }
            else{
                addTileToLists(unit.getRow() + move.x, unit.getCol() + move.y, 0, null);
            }           
        }
        //thoughtful design: are things getting added twice? please check...
        foreach (var move in longMovements)
        {
            for (int i = 1; i <= 8; i++)
            {
                int tempRow = unit.getRow() + move.x * i;
                int tempCol = unit.getCol() + move.y * i;
                //check 2 things: the row/col are in bounds, and the path here isn't blocked
                if (tempRow >= 0 && tempRow <= 7 && tempCol >= 0 && tempCol <= 7)
                {

                    int code = board.newTileRelation(unit, board.board[tempRow, tempCol]);
                    //empty tile. can move here and past it
                    if(code == 0)
                    {
                        addTileToLists(tempRow, tempCol,0,null);
                    } else if(code == 1) //enemy tile. can move here but not past it
                    {
                        addTileToLists(tempRow, tempCol,1,null);
                        break;
                    } else if(code == 2) //friendly tile. can't move here
                    {
                        break;
                    } else if(code == -1) //Opposite king is now in check if moved here
                    {
                        addTileToLists(tempRow, tempCol, -1, null);
                    }
                    
                }
                else break;
                
                //if(board[tempRow, tempCol])
            }
        }
        if (enableSpecial)
        {
            foreach (var move in specialMovements)
            {
                //CALL the validity checker associated with this movement. if true, then it could be valid
                if(move.vc())
                {
                    int tempRow = unit.getRow() + move.x;
                    int tempCol = unit.getCol() + move.y;
                    //we still have to check if the potential location would be in bounds
                    if (tempRow >= 0 && tempRow <= 7 && tempCol >= 0 && tempCol <= 7)
                    {
                        addTileToLists(tempRow, tempCol, 2, move.action);                       
                    }
                }
            }
        }
        return chessTileList;
    }

    public int isKingInCheck()
    {
        listCleanup();
        if(unit.canMove){
        foreach (var move in shortMovements)
        {
            //this is checking if pawn is looking at the king in front of it, it still cant kill unless its diagonal
            if(unit.getPieceType() == Unit.Piece.PAWN){
                if(board.newTileRelation(unit, board.board[unit.getRow() + move.x, unit.getCol() + move.y]) == -1){
                    return 0;
                }
            }
            else{
               int tempRow = unit.getRow() + move.x;
               int tempCol = unit.getCol() + move.y;
               //check 2 things: the row/col are in bounds, and the path here isn't blocked
                if (tempRow >= 0 && tempRow <= 7 && tempCol >= 0 && tempCol <= 7)
                {
                    if(board.newTileRelation(unit, board.board[unit.getRow() + move.x, unit.getCol() + move.y]) == -1){
                        return -1;
                    }
                }
            }           
        }
        //thoughtful design: are things getting added twice? please check...
        foreach (var move in longMovements)
        {
            for (int i = 1; i <= 8; i++)
            {
                int tempRow = unit.getRow() + move.x * i;
                int tempCol = unit.getCol() + move.y * i;
                //check 2 things: the row/col are in bounds, and the path here isn't blocked
                if (tempRow >= 0 && tempRow <= 7 && tempCol >= 0 && tempCol <= 7)
                {
                    int code = board.newTileRelation(unit, board.board[tempRow, tempCol]);
                    if(code == 1) //enemy tile. can move here but not past it
                    {
                        break;
                    } else if(code == 2) //friendly tile. can't move here
                    {
                        break;
                    } else if(code == -1) //Opposite king is now in check if moved here
                    {
                        return -1;
                    }                  
                }
                else break;
            }
        }
        foreach (var move in specialMovements)
            {
                //CALL the validity checker associated with this movement. if true, then it could be valid
                if(move.vc())
                {
                    int tempRow = unit.getRow() + move.x;
                    int tempCol = unit.getCol() + move.y;
                    //we still have to check if the potential location would be in bounds
                    if (tempRow >= 0 && tempRow <= 7 && tempCol >= 0 && tempCol <= 7)
                    {
                        if(board.newTileRelation(unit, board.board[tempRow, tempCol]) == -1){ //King would be in check
                            return -1;
                        }                      
                    }
                }
            }
            }
        return 0;
    }


    //move the tile to an empty destination
    public bool attemptMove(ChessTile destination, LinkedList<(ChessTile tile, int tileType, Special_Action action)> movables)
    {       
        if (validMove(movables, destination)) //was an else if
        {
            Debug.Log("Overriding movement");
            unit.overrideMovement(destination);
            onMove();
            return true;
        }
        return false;
    }

    private void runSpecialAction((ChessTile, int, Special_Action action) move)
    {
        if(move.action != null)
        {
            move.action();
        }
    }

    //basically a rewrite of the "contains" method.
    private bool validMove(LinkedList<(ChessTile tile, int tileType, Special_Action action)> possible, ChessTile dest)
    {
        bool valid = true;
        foreach(var possibleMove in possible){
            //Debug.Log(dest.getName());
            unit.fakeMovement(dest);
            if(unit.getFaction() == Unit.Faction.WHITE){
                GameObject black = GameObject.FindWithTag("blackArmy");
                for(int i = 0; i < black.transform.childCount; i++){
                    if(black.transform.GetChild(i).GetComponent<GeneralMovement>().isKingInCheck() == -1){ //Invalid movement since a piece will have a king in check
                        Debug.Log("White King is in check by " + black.transform.GetChild(i).GetComponent<Unit>().getPieceType());
                        valid = false;
                    }
                }
            }
            else if(unit.getFaction() == Unit.Faction.BLACK){
                GameObject white = GameObject.FindWithTag("whiteArmy");
                for(int i = 0; i < white.transform.childCount; i++){
                    if(white.transform.GetChild(i).GetComponent<GeneralMovement>().isKingInCheck() == -1){ //Invalid movement since a piece will have a king in check
                        Debug.Log("Black King is in check by " + white.transform.GetChild(i).GetComponent<Unit>().getPieceType());
                        valid = false;
                    }
                }
            }
            unit.resetPosition();
            if (possibleMove.tile.getName().Equals(dest.getName()) && valid){
                Debug.Log("Valid move");
                runSpecialAction(possibleMove);
                return true;
            }
        }
        return false;
    }

    public void addShortMovement(int x, int y)
    {
        //no duplicates!
        if (!shortMovements.Contains((x, y)))
        {
            shortMovements.AddFirst((x, y));
        }
    }

    public void removeShortMovement(int x, int y)
    {
        shortMovements.Remove((x, y));
    }

    public void addLongMovement(int x, int y)
    {
        //no duplicates!
        if (!longMovements.Contains((x, y)))
        {
            longMovements.AddFirst((x, y));
        }
    }

    public void removeLongMovement(int x, int y)
    {
        longMovements.Remove((x, y));
    }

    public void addSpecialMovement(int x, int y, Validity_Checker vc, Special_Action action)
    {
        //no duplicates!
        if (!specialMovements.Contains((x, y, vc, action)))
        {
            specialMovements.AddFirst((x, y, vc, action));
        }
    }

    public void removeSpecialMovement(int x, int y, Validity_Checker vc, Special_Action action)
    {
        specialMovements.Remove((x, y, vc, action));
    }

    public void changeMovement(Unit target, Unit.Piece pieceType){ // Given a target piece and the desired type to change it to, this method switches what type it is.
        // AddComponent adds the component needed, setPiece tells the unit what piece it now is (very necessary, otherwise a conflict arises) and Destroy removes a(ny?) type of movement from the target.
        if(target != null){
            Destroy(target.GetComponent<GeneralMovement>());
            switch(pieceType){
                case Unit.Piece.PAWN: //Not sure why you would do this, but maybe in gamemodes where other pieces can change?
                    target.gameObject.AddComponent<MovingPawn>();
                    target.setPiece(Unit.Piece.PAWN);
                    break;
                case Unit.Piece.KNIGHT:
                    target.gameObject.AddComponent<MovingKnight>();
                    target.setPiece(Unit.Piece.KNIGHT);
                    break;
                case Unit.Piece.BISHOP:
                    target.gameObject.AddComponent<MovingBishop>();
                    target.setPiece(Unit.Piece.BISHOP);
                    break;
                case Unit.Piece.ROOK:
                    target.gameObject.AddComponent<MovingRook>();
                    target.setPiece(Unit.Piece.ROOK);
                    break;
                case Unit.Piece.QUEEN:
                    target.gameObject.AddComponent<MovingQueen>();
                    target.setPiece(Unit.Piece.QUEEN);
                    break;
                case Unit.Piece.KING: //Should not happen, but may be useful for gamemodes. BE CAREFUL, THIS WILL PROBABLY CHANGE WIN CONDITION UNPREDICTABLY
                    target.gameObject.AddComponent<MovingKing>();
                    target.setPiece(Unit.Piece.KING);
                    break;
                default:
                    target.gameObject.AddComponent<MovingPawn>(); //To prevent bugs, give it at least pawn movement
                    target.setPiece(Unit.Piece.PAWN);
                    Debug.Log("Default case for changing movement");
                    break;
            }
        }      
    }
}
