using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
//
public class BoardCube : MonoBehaviour
{
    public GameManager gameManager;
    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        gameManager.moveSelectedPieceTo(gameObject);
    }
}
