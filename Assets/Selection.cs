using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public TextMeshProUGUI TooltipHover;
    public TextMeshProUGUI TooltipSelected;
    public TextMeshProUGUI TooltipPiece;

    private GameManager gameManager;

    //used for displaying "ghost tiles", places where currently selected piece could move.
    private ChessBoardSetup board;
    private GameObject ghostTile;
    private GameObject ghostTileEnemy;
    private GameObject ghostTileSpecial;
    private GameObject ghostTileCheck;
    private LinkedList<GameObject> ghostTileList = new LinkedList<GameObject>();
    public LinkedList<(ChessTile tile,int tileType)> movables = new LinkedList<(ChessTile tile, int tileType)>();
    private bool whichTurnIsIt; //Check for which turn it is, so you can defeat pieces.

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<ChessBoardSetup>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // this is a LAZY AF way of updating the text in the top left, we can improve it later if needed
    void Update()
    {        
    }

    public void updateUI(){
        if (gameManager.selectedTile != null)
        {
            TooltipSelected.text = "Selected: " + gameManager.selectedTile.getName();
        }
        else
        {
            TooltipSelected.text = "Selected: None";
        }
        if (gameManager.playerHasPiece)
        {
            TooltipPiece.text = "Piece: " + gameManager.playerPiece.getFaction().ToString() + " " + 
                gameManager.playerPiece.getPieceType();
        } else
        {
            TooltipPiece.text = "Piece: None";
        }
    }

    //enable and move the shiny selector box to the specified location
    public void setSelectorPosition(ChessTile ct)
    {
        gameObject.SetActive(true);
        Vector3 pos = ct.transform.position;
        pos.y = 0.1f;
        gameObject.transform.position = pos;

        TooltipHover.text = "Hovering: " + ct.getName();
    }

    //disable the shiny selector box
    public void resetSelectorPosition()
    {
        gameObject.SetActive(false);

        TooltipHover.text = "Hovering: None";
    }




    //select the piece at this position
    public void selectPiece(ChessTile ct)
    {
        //if(occupant.getFaction() == Player Faction) will be used for checking if you have selected your own pieces
        if(gameManager.gameState == GameManager.GameState.PLAYERONETURN){
            Debug.Log("Player One Turn");
            if (!gameManager.playerHasPiece)
            { // Player One currently does not have a piece selected
                if(ct.occupant.GetComponent<Unit>().getFactionString() == "white"){
                    gameManager.playerHasPiece = true; //Now they do
                    gameManager.playerPiece = ct.occupant.GetComponent<Unit>();
                    gameManager.selectedTile = ct;
                    makeGhostTiles(ct.occupant);
                    updateUI();
                }
                else{
                    Debug.Log("Wrong team!");
                }
                
            }
            else
            { // Player One has a piece already, so we need to switch them or deselect
                if (ct.Equals(gameManager.selectedTile)) //same tile. deselect
                {
                   Debug.Log("deselect");
                   deselectPiece();
                   makeGhostTiles(ct.occupant);
                }
                else if(ct.occupant.getFactionString() != "white"){ //Player one turn, hitting a black piece
                    //Need to check what piece we have. Pawn and King have unique checks (pawns can't attack forward, king's can't attack if it puts you in check)
                    if(gameManager.playerPiece.getPieceType() == Unit.Piece.KING){ //We have a king
                        //King should not be able to attack if it will be in danger for doing so
                        //Need to check if the move is castling. If it is, move both king and the selected rook
                        emptySelection(ct);
                    }
                    else{ //Generic piece type
                         emptySelection(ct);
                    }
                }
                else //different tile? select the new one
                {
                    Debug.Log("Switching pieces " + ct.getName());
                    gameManager.playerPiece = ct.occupant.GetComponent<Unit>();
                    gameManager.selectedTile = ct;
                    makeGhostTiles(ct.occupant);
                }            
            }
        }
        else if(gameManager.gameState == GameManager.GameState.PLAYERTWOTURN){
            Debug.Log("Player Two Turn");
            if (!gameManager.playerHasPiece)
            { // Player Two currently does not have a piece selected
                if(ct.occupant.GetComponent<Unit>().getFactionString() == "black"){
                    gameManager.playerHasPiece = true; //Now they do
                    gameManager.playerPiece = ct.occupant.GetComponent<Unit>();
                    gameManager.selectedTile = ct;
                    makeGhostTiles(ct.occupant);
                }
                else{
                    Debug.Log("Wrong team!");
                }
            }
            else
            { // Player One has a piece already, so we need to switch them or deselect
                if (ct.Equals(gameManager.selectedTile)) //same tile. deselect
                {
                   Debug.Log("deselect");
                   deselectPiece();
                   makeGhostTiles(ct.occupant);
                }
                else if(gameManager.playerOneTurn && ct.occupant.getFactionString() != "black"){ //Player two turn, hitting a white piece
                    //Need to check what piece we have. Pawn and King have unique checks (pawns can't attack forward, king's can't attack if it puts you in check)
                    if(gameManager.playerPiece.getPieceType() == Unit.Piece.KING){ //We have a king
                        //King should not be able to attack if it will be in danger for doing so
                        emptySelection(ct);
                    }
                    else{ //Generic piece type
                        emptySelection(ct);
                    }
                }
                else //different tile? select the new one
                {
                    Debug.Log("Switching pieces " + ct.getName());
                    gameManager.playerPiece = ct.occupant.GetComponent<Unit>();
                    gameManager.selectedTile = ct;
                    makeGhostTiles(ct.occupant);
                }            
            }
        }
        updateUI();       
    }

    //deselect the piece at this position
    public void deselectPiece()
    {
        cleanupGhostTile();
        gameManager.selectedTile = null;
        gameManager.playerHasPiece = false;
        gameManager.playerPiece = null;
    }

    public void emptySelection(ChessTile ct)
    {
        gameManager.selectedTile = ct;

        if (gameManager.playerHasPiece)
        {
            if (gameManager.playerPiece.GetComponent<GeneralMovement>().attemptMove(ct,movables))
            {
                //Invert turn
                gameManager.changeGameState();
                //delete ghost tiles, deselect piece just moved
                deselectPiece();
                updateUI();
            }
        }
    }





    //ghost tile stuff
    private void makeGhostTiles(Unit unit)
    {
        if (ghostTile == null) { ghostTile = GameObject.FindWithTag("ghostTile"); }
        if (ghostTileEnemy == null) { ghostTileEnemy = GameObject.FindWithTag("ghostTileEnemy"); }
        if (ghostTileSpecial == null) { ghostTileSpecial = GameObject.FindWithTag("ghostTileSpecial"); }
        if (ghostTileCheck == null) { ghostTileCheck = GameObject.FindWithTag("ghostTileCheck"); }
        //Debug.Log(unit.getPieceType());
        cleanupGhostTile();
        movables = unit.GetComponent<GeneralMovement>().getPossibleMoves(true);
        
        foreach (var chessTile in movables)
        {
            if(gameManager.gameState == GameManager.GameState.PLAYERONETURN){ //White chess is playing, should not show valid move if its on white piece (UNLESS CASTLING)
                if(chessTile.tileType == 2){
                    //Debug.Log(chessTile.tileType);
                    addToGhostList(chessTile.tile.row, chessTile.tile.colNum,2); //Special tile
                }
                else if(board.board[chessTile.tile.row,chessTile.tile.colNum].GetComponent<ChessTile>().occupant == null){
                    addToGhostList(chessTile.tile.row, chessTile.tile.colNum,0); //Number indicates its a blue ghost tile
                }
                else if(board.board[chessTile.tile.row,chessTile.tile.colNum].GetComponent<ChessTile>().occupant.GetComponent<Unit>().getFactionString() != "white"){
                    //Debug.Log("Looking at enemy for white");
                    addToGhostList(chessTile.tile.row, chessTile.tile.colNum,1); //Number indicates its a red ghost tile (for enemy)
                }
            }
            else if(gameManager.gameState == GameManager.GameState.PLAYERTWOTURN){ //Black chess is playing, should not show valid move if on a black piece (UNLESS CASTLING)
                if(chessTile.tileType == 2){
                    addToGhostList(chessTile.tile.row, chessTile.tile.colNum,2); //Special tile
                }
                else if(chessTile.tileType == -1){ //King in check
                    addToGhostList(chessTile.tile.row, chessTile.tile.colNum,-1); //King in check
                }
                else if(board.board[chessTile.tile.row,chessTile.tile.colNum].GetComponent<ChessTile>().occupant == null){
                    addToGhostList(chessTile.tile.row, chessTile.tile.colNum,0); //Number indicates its a blue ghost tile
                }
                else if(board.board[chessTile.tile.row,chessTile.tile.colNum].GetComponent<ChessTile>().occupant.GetComponent<Unit>().getFactionString() != "black"){
                    //Debug.Log("Looking at enemy for black");
                    addToGhostList(chessTile.tile.row, chessTile.tile.colNum,1); //Number indicates its a red ghost tile (for enemy)
                }
            }
            
        }
    }

    private void addToGhostList(int x, int y, int tileType)
    {
        GameObject gt;
        switch(tileType)
        {
            case -1: //pink tile for king in check from this move
            gt = GameObject.Instantiate(ghostTileSpecial);
            break;
            case 0: //add to ghost tile list (blue square object showing where you can move)
            gt = GameObject.Instantiate(ghostTile);
            break;
            case 1: //red tile for enemy
            gt = GameObject.Instantiate(ghostTileEnemy);
            break;
            case 2: //yellow tile for special move
            gt = GameObject.Instantiate(ghostTileSpecial);
            break;
            default: //Shouldn't occur, may be responsible for extra tiles if done incorrectly
            gt = GameObject.Instantiate(ghostTile);
            break;
        }
        
        Vector3 oldPos = board.board[x, y].transform.position;
        gt.transform.position = new Vector3(oldPos.x, 0.5f, oldPos.z);
        ghostTileList.AddFirst(gt);
    }

    private void cleanupGhostTile()
    {
        foreach (GameObject g in ghostTileList)
        {
            Destroy(g);
        }
        ghostTileList.Clear();
    }
}
