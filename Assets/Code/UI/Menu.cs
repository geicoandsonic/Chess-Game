using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject[] childMenus; // Menu's that are children of this menu
    public GameObject[] siblingMenus; //Menu's that occupy the same tier as one another
    public Button exitButton;
    // Start is called before the first frame update

    public void closeChildren(){
        if(childMenus != null){
            foreach(var menu in childMenus){
                menu.GetComponent<Menu>().closeChildren();
                menu.SetActive(false);
            }
        }
    }

    public void closeSelf(){
        Debug.Log("Closing self");
        closeChildren();
        this.gameObject.SetActive(false);
    }

    public void closeSibling(){
        if(siblingMenus != null){
            foreach(var menu in siblingMenus){
                menu.GetComponent<Menu>().closeChildren();
                menu.SetActive(false);
            }
        }
    }

    public void openChild(int index){
        if(childMenus[index] != null){
            childMenus[index].SetActive(true);
            childMenus[index].GetComponent<Menu>().closeSibling();
        }
    }

    public void closeChild(int index){
        if(childMenus[index] != null){
            childMenus[index].SetActive(false);
        }
    }
}
