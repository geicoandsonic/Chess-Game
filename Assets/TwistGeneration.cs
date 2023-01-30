using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TwistGeneration : MonoBehaviour
{
    //realistically, there should be no more than 3 twists in a game, otherwise it's complete chaos
    public int num_twists = 3;
    public Image[] chosenTwistIcons;
    public Sprite[] twistIcons;
    public TextMeshProUGUI[] twistNames;

    //currently there are: SEE WILDCARDS
    // 29 twists
    public enum Twist {REVOLUTION, CTF, KOTH, WIPEOUT, NO_MANS_LAND, TRENCHES, PESTILENCE, MINESWEEPER,
        SNAKE, X2LONG, X2WIDE, HEARTS_AND_MINDS, BATTLE_ROYALE, WEATHER, FOG_OF_WAR, BOXING, MIRROR,
        BLACK_HOLE, WATER, RIVER, ONE_TWO_PUNCH, ASH_AND_SNOW, NECRONOMICON, STALIN, MERCS,
        ATTRITION, MIAMI, HORDE, ROULETTE};

    //short names for each twist. temporary, will eventually be replaced. SEE WILDCARDS
    private string[] temporaryNames = { "Rev", "CTF", "KOTH", "Wipeout", "NML", "Trench", "Plague", "Sweep",
    "Snake", "Long", "Wide", "H&M", "B.R.", "Weather", "FogOfWar", "Boxing", "Mirror",
    "B.H.", "10in", "River", "1-2", "A&S", "Necro", "Stalin", "Mercs", "Attr.", "Miami", "Undead", "Roulette"};

    public Twist[] chosenTwists;

    // Start is called before the first frame update
    void Start()
    {
        //choose the twists to be used
        //TODO: fix possibility of same twist being chosen twice
        chosenTwists = new Twist[num_twists];
        for(int i=0; i<num_twists; i++)
        {
            int sel = (int) Mathf.Floor(Random.Range(0.0f, 28.999f));
            //get element of enum by index
            Twist t = (Twist)sel;
            chosenTwists[i] = t;

            //assign pictures in the UI
            chosenTwistIcons[i].sprite = twistIcons[sel];
            twistNames[i].text = temporaryNames[sel];
            //TODO: actually apply effects of the twists in code as well...
            //   (probably each having their own script to run.)
            
        }

        


        
    }
}
