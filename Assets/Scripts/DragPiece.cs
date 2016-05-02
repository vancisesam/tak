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
    public bool isWall = false;

    public float deltaHeight = 0;
    private float myHeight;
    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pieceHolder = transform.parent;
        myHeight = gameObject.GetComponent<Renderer>().bounds.size.x / 2.0f - .1f;
    }

    void OnMouseDown()
    {
        
        gameManager.selectPiece(gameObject);
        
    }
    public void makeWall()
    {
        isWall = true;
        deltaHeight = myHeight;
        transform.position += new Vector3(0, deltaHeight);
        transform.rotation = Quaternion.Euler(90, 0, 0);

    }
    public void crushWall()
    {
        isWall = false;
        transform.position += new Vector3(0, -deltaHeight);
        deltaHeight = 0.0f;
        transform.rotation = Quaternion.Euler(0, 0, 0);

    }




}
