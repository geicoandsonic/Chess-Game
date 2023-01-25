using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public TextMeshProUGUI tooltip;

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

    // Update is called once per frame
    void Update()
    {
        
    }




    //enable and move the shiny selector box to the specified location
    public void setSelectorPosition(ChessTile ct)
    {
        gameObject.SetActive(true);
        Vector3 pos = ct.transform.position;
        pos.y = 0.1f;
        gameObject.transform.position = pos;

        tooltip.text = "Tile: " + ct.getName() + " (" + ct.getOccupantName() + ")";
    }

    //disable the shiny selector box
    public void resetSelectorPosition()
    {
        gameObject.SetActive(false);

        tooltip.text = "Tile: None";
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
                //gameManager.selectedTile.occupant = null;

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
            addToGhostList(tile.row, tile.colNum);
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
