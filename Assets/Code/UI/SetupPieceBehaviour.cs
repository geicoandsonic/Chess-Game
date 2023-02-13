using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupPieceBehaviour : MonoBehaviour
{
    public Sprite pieceImage;
    [SerializeField] private Image piecePanel;
    [SerializeField] private GameObject panel;

    public void SetupMenu(){
        piecePanel.sprite = pieceImage;
        panel.SetActive(true);
    }
}
