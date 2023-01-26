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
    private LinkedList<GameObject> ghostTileList = new LinkedList<GameObject>();
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
        if (gameManager.selectedTile != null)
        {
            TooltipSelected.text = "Selected: " + gameManager.selectedTile.getName();
        }
        else
        {
            TooltipSelected.text = "Selected: None";
        }

        
        if (gameManager.playerOneHasPiece)
        {
            TooltipPiece.text = "Piece: " + gameManager.playerOnePiece.getFaction().ToString() + " " + 
                gameManager.playerOnePiece.getPieceType();
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
        if (!gameManager.playerOneHasPiece)
        { // Player One currently does not have a piece selected
            gameManager.playerOneHasPiece = true; //Now they do
            gameManager.playerOnePiece = ct.occupant.GetComponent<Unit>();
            gameManager.selectedTile = ct;
            makeGhostTiles(ct.occupant);
        }
        else
        { // Player One has a piece already, so we need to switch them or deselect
            if (ct.Equals(gameManager.selectedTile)) //same tile. deselect
            {
                Debug.Log("deselect");
                deselectPiece();
                makeGhostTiles(ct.occupant);
            }
            else if(gameManager.playerOneTurn && ct.occupant.getFactionString() != "white"){ //Player one turn, hitting a black piece
                //Need to check what piece we have. Pawn and King have unique checks (pawns can't attack forward, king's can't attack if it puts you in check)
                if(gameManager.playerOnePiece.getPieceType() == Unit.Piece.PAWN){ //We have a pawn
                    
                }
                else if(gameManager.playerOnePiece.getPieceType() == Unit.Piece.KING){ //We have a king
                    
                }
                else{ //Generic piece type
                    Debug.Log("Destroying");
                    Destroy(ct.occupant.gameObject);
                    ct.occupant = null;
                    emptySelection(ct);
                }
            }
            else //different tile? select the new one
            {
                Debug.Log("Switching pieces " + ct.getName());
                gameManager.playerOnePiece = ct.occupant.GetComponent<Unit>();
                gameManager.selectedTile = ct;
                makeGhostTiles(ct.occupant);
            }
            

        }
        
    }

    //deselect the piece at this position
    public void deselectPiece()
    {
        cleanupGhostTile();
        gameManager.selectedTile = null;
        gameManager.playerOneHasPiece = false;
        gameManager.playerOnePiece = null;
    }

    public void emptySelection(ChessTile ct)
    {
        gameManager.selectedTile = ct;

        if (gameManager.playerOneHasPiece)
        {
            Debug.Log("Taking move " + ct.getName());
            if (gameManager.playerOnePiece.takeMove(ct.row, ct.colNum, ct))
            {
                //Invert turn
                gameManager.playerOneTurn = !gameManager.playerOneTurn;
                //delete ghost tiles, deselect piece just moved
                cleanupGhostTile();
                deselectPiece();
            }
        }
    }





    //ghost tile stuff
    private void makeGhostTiles(Unit unit)
    {
        if (ghostTile == null) { ghostTile = GameObject.FindWithTag("ghostTile"); }
        if (ghostTileEnemy == null) { ghostTileEnemy = GameObject.FindWithTag("ghostTileEnemy"); }
        Debug.Log(unit.getPieceType());
        cleanupGhostTile();
        LinkedList<ChessTile> movables = unit.GetComponent<GeneralMovement>().getPossibleMoves();

        foreach (var tile in movables)
        {
            if(gameManager.playerOneTurn){ //White chess is playing, should not show valid move if its on white piece (UNLESS CASTLING)
                if(board.board[tile.row,tile.colNum].GetComponent<ChessTile>().occupant == null){
                    addToGhostList(tile.row, tile.colNum,0); //Number indicates its a blue ghost tile
                }
                else if(board.board[tile.row,tile.colNum].GetComponent<ChessTile>().occupant.GetComponent<Unit>().getFactionString() != "white"){
                    Debug.Log("Looking at enemy for white");
                    addToGhostList(tile.row, tile.colNum,1); //Number indicates its a red ghost tile (for enemy)
                }
            }
            else{ //Black chess is playing, should not show valid move if on a black piece (UNLESS CASTLING)
                if(board.board[tile.row,tile.colNum].GetComponent<ChessTile>().occupant == null){
                    addToGhostList(tile.row, tile.colNum,0); //Number indicates its a blue ghost tile
                }
                else if(board.board[tile.row,tile.colNum].GetComponent<ChessTile>().occupant.GetComponent<Unit>().getFactionString() != "black"){
                    Debug.Log("Looking at enemy for black");
                    addToGhostList(tile.row, tile.colNum,1); //Number indicates its a red ghost tile (for enemy)
                }
            }
            
        }
    }

    private void addToGhostList(int x, int y, int tileType)
    {
        GameObject gt;
        switch(tileType)
        {
            case 0: //add to ghost tile list (blue square object showing where you can move)
            gt = GameObject.Instantiate(ghostTile);
            break;
            case 1: //red tile for enemy
            Debug.Log("Enemy tile");
            gt = GameObject.Instantiate(ghostTileEnemy);
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
