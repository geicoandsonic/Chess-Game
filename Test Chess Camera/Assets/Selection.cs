using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public TextMeshProUGUI tooltip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSelectorPosition(ChessTile ct)
    {
        gameObject.SetActive(true);
        Vector3 pos = ct.transform.position;
        pos.y = 0.1f;
        gameObject.transform.position = pos;

        tooltip.text = "Tile: " + ct.getName() + " (" + ct.getOccupantName() + ")";
    }

    public void resetSelectorPosition()
    {
        gameObject.SetActive(false);

        tooltip.text = "Tile: None";
    }
}
