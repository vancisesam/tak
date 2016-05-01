using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DragPiece : MonoBehaviour
{
    public bool inStack;
    public GameManager gameManager;
    public Transform pieceHolder;
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
