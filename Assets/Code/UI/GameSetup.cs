using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject gameSetupMenu;
    public GameObject pieceSetupMenu; //Should have children of type pieceMenu that is piece specific.
    public GameObject mapSetupMenu;
    public GameObject[] mapMenuButtons;
    public ChessBoardSetup board;
    // Start is called before the first frame update
    void Start()
    {
        startScreen.SetActive(true);
    }

    public void openStart(){
        startScreen.SetActive(true);
    }

    public void closeStart(){
        startScreen.SetActive(false);
    }

    public void openGameSetup(){
        gameSetupMenu.SetActive(true);
        closeStart();
    }

    public void openPieceSetup(){
        pieceSetupMenu.SetActive(true);
    }

    public void mapSetup(){
        Debug.Log("Map creating");
        board.mapCreating = true;     
    }

    public void turnOffButtons(){
        foreach(var obj in mapMenuButtons){
            obj.SetActive(false);
        }
    }

    public void turnOnButtons(){
        foreach(var obj in mapMenuButtons){
            obj.SetActive(true);
        }
    }

    public void StartGame(){
        board.mapCreating = false;
        board.DontDestroy();
        SceneManager.LoadScene("Chess");
    }

    public void exitGame(){
        Application.Quit();
    }
}
