using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whiteOrBlack : MonoBehaviour
{
    public MapCreator map;
    [SerializeField] private bool white;
    void Awake(){
        map = GameObject.Find("ChessTilesPanel").GetComponent<MapCreator>();
    }

    public void send(){
        if(white){
            map.selectFaction(Unit.Faction.WHITE);
        }
        else{
            map.selectFaction(Unit.Faction.BLACK);
        }
    }
}
