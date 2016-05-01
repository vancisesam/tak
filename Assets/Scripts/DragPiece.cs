using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DragPiece : MonoBehaviour
{
    
    public GameManager gameManager;
    public Transform pieceHolder;


    public bool inStack;    //if a piece is off the board, waiting to be played
    public bool isMovable;
    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pieceHolder = transform.parent;
    }

    void OnMouseDown()
    {
        
        gameManager.selectPiece(gameObject);
        
    }




}
