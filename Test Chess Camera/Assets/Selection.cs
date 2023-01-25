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
    private LinkedList<GameObject> ghostTileList = new LinkedList<GameObject>();

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
        }
        else
        { // Player One has a piece already, so we need to switch them or deselect
            if (ct.Equals(gameManager.selectedTile)) //same tile. deselect
            {
                Debug.Log("deselect");
                deselectPiece();
            } else //different tile? select the new one
            {
                Debug.Log("Switching pieces " + ct.getName());
                gameManager.playerOnePiece = ct.occupant.GetComponent<Unit>();
                gameManager.selectedTile = ct;
            }
            

        }
        makeGhostTiles(ct.occupant);
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

        cleanupGhostTile();
        LinkedList<ChessTile> movables = unit.GetComponent<GeneralMovement>().getPossibleMoves();

        foreach (var tile in movables)
        {
            if(gameManager.playerOneTurn){ //White chess is playing, should not show valid move if its on white piece (UNLESS CASTLING)
                if(board.board[tile.row,tile.colNum].GetComponent<ChessTile>().occupant == null){
                    addToGhostList(tile.row, tile.colNum);
                }
                else if(board.board[tile.row,tile.colNum].GetComponent<ChessTile>().occupant.GetComponent<Unit>().getFactionString() != "white"){
                    addToGhostList(tile.row, tile.colNum);
                }
            }
            else{ //Black chess is playing, should not show valid move if on a black piece (UNLESS CASTLING)
                if(board.board[tile.row,tile.colNum].GetComponent<ChessTile>().occupant == null){
                    addToGhostList(tile.row, tile.colNum);
                }
                else if(board.board[tile.row,tile.colNum].GetComponent<ChessTile>().occupant.GetComponent<Unit>().getFactionString() != "black"){
                    addToGhostList(tile.row, tile.colNum);
                }
            }
            
        }
    }

    private void addToGhostList(int x, int y)
    {

        //add to ghost tile list (blue square object showing where you can move)
        GameObject gt = GameObject.Instantiate(ghostTile);
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
