using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour
{
    public GameObject uiMenu;
    public GameManager gameManager; 
    public GameObject deleteButton; //used to change colors
    public GameObject moveButton;
    public Sprite whiteSquare;
    public Sprite redSquare;
    public Sprite orangeSquare;
    public bool moveEnabled;
    public bool deleteEnabled;
    public ChessBoardSetup board;

    void Start(){
        board = FindObjectOfType<ChessBoardSetup>();
    }

    public void enableMenu(){
        if(deleteEnabled){ //Need to disable moveFirst
            deleteEnabled = false;
            deleteButton.GetComponent<Image>().sprite = whiteSquare;
        }
        if(moveEnabled){ //Need to disable moveFirst
            moveEnabled = false;
            moveButton.GetComponent<Image>().sprite = whiteSquare;            
        }
        gameManager.setDebugGameState();
        uiMenu.SetActive(!uiMenu.activeSelf);
    }

    public void interactMovement(){
        if(deleteEnabled){ //Need to disable moveFirst
            deleteEnabled = false;
            deleteButton.GetComponent<Image>().sprite = whiteSquare;
            gameManager.setDebugGameState();
        }       
        if(!moveEnabled){
            gameManager.debugMove();
            moveButton.GetComponent<Image>().sprite = orangeSquare;
            moveEnabled = true;
        }
        else{
            gameManager.setDebugGameState();
            moveButton.GetComponent<Image>().sprite = whiteSquare;
            moveEnabled = false;
        }
    }

    public void interactDelete(){
        if(moveEnabled){ //Need to disable moveFirst
            moveEnabled = false;
            moveButton.GetComponent<Image>().sprite = whiteSquare;
            gameManager.setDebugGameState();
        }
        if(!deleteEnabled){
            gameManager.debugDelete();
            deleteButton.GetComponent<Image>().sprite = redSquare;
            deleteEnabled = true;
        }
        else{
            gameManager.setDebugGameState();
            deleteButton.GetComponent<Image>().sprite = whiteSquare;
            deleteEnabled = false;
        }
    }

    public void interactReload(){
        moveEnabled = false;
        moveButton.GetComponent<Image>().sprite = whiteSquare;
        deleteEnabled = false;
        deleteButton.GetComponent<Image>().sprite = whiteSquare;
        gameManager.restart();
        board.restart();
    }
}
