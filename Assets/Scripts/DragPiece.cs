using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DragPiece : MonoBehaviour
{
    public GameManager gameManager;
    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnMouseDown()
    {
        gameManager.selectPiece(gameObject);
    }




}
