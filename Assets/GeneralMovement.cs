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
     queens have all 8 of these.*/
    public LinkedList<(int x, int y)> shortMovements = new LinkedList<(int x, int y)>();
    public LinkedList<(int x, int y)> longMovements = new LinkedList<(int x, int y)>();
    public LinkedList<(int x, int y)> specialMovements = new LinkedList<(int x, int y)>();
    //[SerializeField] private GameObject ghostTile;
    [SerializeField] private Unit unit;
    private ChessBoardSetup board;

    //private LinkedList<GameObject> ghostTileList = new LinkedList<GameObject>();
    private LinkedList<ChessTile> chessTileList = new LinkedList<ChessTile>();

    public virtual void moveSetup()
    {
        //override and add "default" movements of the piece here.
    }

    public virtual void onMove()
    {
        //override with what this piece does, if anything, when it moves.
    }


    //add to chess tile list (list of chessTiles for your perusal)
    protected void addTileToLists(int upBy, int rightBy)
    {
        if(upBy >= 0 && upBy <= 7 && rightBy >= 0 && rightBy <= 7){
            chessTileList.AddFirst(board.board[upBy,rightBy]);
        }
        
    }

    //clear the chess tile list
    protected void listCleanup()
    {

        chessTileList.Clear();
    }

    //get all possible tiles this piece could move to next turn.
    public LinkedList<ChessTile> getPossibleMoves()
    {
        unit = GetComponent<Unit>();
        if(board == null) { board = FindObjectOfType<ChessBoardSetup>(); }
        listCleanup();
        foreach (var move in shortMovements)
        {
            addTileToLists(unit.getRow() + move.x, unit.getCol() + move.y);
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
                        addTileToLists(tempRow, tempCol);
                    } else if(code == 1) //enemy tile. can move here but not past it
                    {
                        addTileToLists(tempRow, tempCol);
                        break;
                    } else if(code == 2) //friendly tile. can't move here
                    {
                        break;
                    }
                    
                }
                else break;
                
                //if(board[tempRow, tempCol])
            }
        }
        return chessTileList;
        
    }

    public LinkedList<ChessTile> getPossibleMoves(bool enableSpecial)
    {
        unit = GetComponent<Unit>();
        if(board == null) { board = FindObjectOfType<ChessBoardSetup>(); }
        listCleanup();
        foreach (var move in shortMovements)
        {
            //this is adding extra movement for pawn so the middle piece reports as an enemy.
            if(unit.getPieceType() == Unit.Piece.PAWN){
                if(board.board[unit.getRow() + move.x, unit.getCol() + move.y].occupant == null){
                    addTileToLists(unit.getRow() + move.x, unit.getCol() + move.y);
                }
            }
            else{
                addTileToLists(unit.getRow() + move.x, unit.getCol() + move.y);
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
                        addTileToLists(tempRow, tempCol);
                    } else if(code == 1) //enemy tile. can move here but not past it
                    {
                        addTileToLists(tempRow, tempCol);
                        break;
                    } else if(code == 2) //friendly tile. can't move here
                    {
                        break;
                    }
                    
                }
                else break;
                
                //if(board[tempRow, tempCol])
            }
        }
        if(enableSpecial){
            switch(unit.getPieceType()){
                case Unit.Piece.PAWN:
                    foreach (var move in specialMovements){
                            int tempRow = unit.getRow() + move.x;
                            int tempCol = unit.getCol() + move.y; //Start at unit col over to the right one, the middle, then left
                            //check 2 things: the row/col are in bounds, and the path here isn't blocked
                            if (tempRow >= 0 && tempRow <= 7 && tempCol >= 0 && tempCol <= 7)
                            {
                                
                                int code = board.newTileRelation(unit, board.board[tempRow, tempCol]);
                                //empty tile. can move here and past it
                                if(code == 0 || code == 1 || code == 3) //1, enemy tile. can move here but not past it, probably shouldn't happen for a pawn 3, special case, can attack
                                {
                                    addTileToLists(tempRow, tempCol);
                                }                    
                             }
                            else break;
                    }
                    break;
                case Unit.Piece.KING: //Castling, King's trigger castling
                    foreach (var move in specialMovements){
                        int tempRow = unit.getRow() + move.x;
                        int tempCol = unit.getCol() + move.y;
                        int code;
                        bool castling = false;
                        if(move.y == 2 || move.y == -2){
                            castling = true;
                        }
                        //check 2 things: the row/col are in bounds, and the path here isn't blocked
                        if (tempRow >= 0 && tempRow <= 7 && tempCol >= 0 && tempCol <= 7)
                        {                             
                           if(castling){ //Need to check that tiles between the target rook are empty
                                if(move.y < 0){
                                    for(int i = 1; i < 2; i++){
                                        code = board.newTileRelation(unit, board.board[tempRow, (unit.getCol() + i)]);
                                        if(code != 0){
                                            castling = false;
                                            break;
                                        }
                                    }
                                }
                                else if(move.y > 0){
                                    for(int i = -1; i > -3; i--){
                                        code = board.newTileRelation(unit, board.board[tempRow, (unit.getCol() + i)]);
                                        if(code != 0){
                                            castling = false;
                                            break;
                                        }
                                    }
                                }
                                if(castling){ //Having passed the above two, there are no blocks between the king and rook.
                                    Debug.Log("Castling passed first test, checking rook qualification");
                                    if(move.y < 0){ //Going right
                                        code = board.newTileRelation(unit, board.board[tempRow, unit.getCol() + 3]);
                                        //empty tile. can move here and past it
                                        if(code == 3) //3 is for castling, only valid if both king and rook have not moved. Both this unit and the other need to move at the same time
                                        {
                                            Debug.Log("Castle valid");
                                            addTileToLists(tempRow, unit.getCol() + 2);
                                        }
                                    }
                                    else if(move.y > 0){ //Going left
                                        code = board.newTileRelation(unit, board.board[tempRow, unit.getCol() - 4]);
                                        //empty tile. can move here and past it
                                        if(code == 3) //3 is for castling, only valid if both king and rook have not moved. Both this unit and the other need to move at the same time
                                        {
                                            Debug.Log("Castle valid");
                                            addTileToLists(tempRow, unit.getCol() - 2);
                                        }
                                    }
                                    
                                }
                                
                                
                           }
                                               
                        }
                        else break;
                    }
                    break;
                default:
                break;   
                }
        }
        return chessTileList;
    }


    //move the tile to an empty destination
    public bool attemptMove2(ChessTile destination)
    {
        if(unit.getPieceType() == Unit.Piece.PAWN){ //Pawn has special cases, shouldn't add moves that won't normally work (diagonals)
            if(destination.occupant == null){ //Pawn is not going to another unit, so it can only move forward, same as the else if below
                if (validMove(getPossibleMoves(), destination))
                {
                    onMove(); //Probably write changeMovement in onMove provided it hits a spot to change it, and get the input from a button
                    Debug.Log("new move method worked, path found for a pawn");
                    return true;
                 }
                 return false;
            }
            else{ //Pawn is seeing another unit (HAS NOT DIFFERENTIATED WHITE OR BLACK YET)
                if (validMove(getPossibleMoves(true), destination))
                {
                    onMove();
                    Debug.Log("new move method worked, path found for a pawn, special move");
                    return true;
                 }
                 return false;
            }
        }
        else if(unit.getPieceType() == Unit.Piece.KING){
            if (validMove(getPossibleMoves(true), destination))
                {
                    onMove();
                    Debug.Log("new move method worked, path found for a king, special move");
                    return true;
                 }
                 return false;
        }
        else if (validMove(getPossibleMoves(), destination))
        {
            onMove();
            Debug.Log("new move method worked, path found");
            return true;
        }
        return false;
    }

    //basically a rewrite of the "contains" method.
    private bool validMove(LinkedList<ChessTile> possible, ChessTile dest)
    {
        foreach(ChessTile possibleMove in possible){
            Debug.Log(dest.getName());
            if (possibleMove.getName().Equals(dest.getName())) return true;
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

    public void addSpecialMovement(int x, int y){
        //no duplicates!
        if (!specialMovements.Contains((x, y)))
        {
            specialMovements.AddFirst((x, y));
        }
    }

    public void removeSpecialMovement(int x, int y)
    {
        specialMovements.Remove((x, y));
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
