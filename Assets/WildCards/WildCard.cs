using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "WildCards", order = 1)]
public class WildCard : ScriptableObject
{
    public string cardName;
    public string description;
    public bool effectsParticularPiece; //if this card changes a specific unit type
    public Unit piece;
    //Make effect enum or something here
    public bool callWildCard(){
        if(effectsParticularPiece){ //Change attribute of piece
            //Not super sure what to do here...
        }
        return true; //temp
    }
}
