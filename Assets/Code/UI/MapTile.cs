using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class MapTile : MonoBehaviour
{
    public int row;
    public int col;
    public MapCreator map;
    void Awake(){
        map = GameObject.Find("ChessTilesPanel").GetComponent<MapCreator>();
    }
    public void sendTileInfo(){
        map.updateMap(row,col);
    }
}
