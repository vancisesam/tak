using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour {

    private GameObject selected = null;
    public Material selectedMaterial;
    private Material materialOfSelected;

    public enum turn { round, square};
    public turn currentTurn=turn.round;
    GameObject[] playersPieces;
    GameObject[] squarePieces;
    GameObject[] roundPieces;
    DragPiece[] squareData;
    DragPiece[] roundData;
    // Use this for initialization
    void Start () {
        roundPieces = GameObject.FindGameObjectsWithTag("roundPiece");
        squarePieces = GameObject.FindGameObjectsWithTag("squarePiece");
        int i = 0;
        roundData = new DragPiece[roundPieces.Length];
        squareData = new DragPiece[squarePieces.Length];
        foreach (GameObject piece in roundPieces)
        {
            roundData[i] = roundPieces[i].GetComponent<DragPiece>();
            squareData[i] = squarePieces[i].GetComponent<DragPiece>();
            i++;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)){
            //deselectPiece();
        }
	}

    public void moveSelectedPieceTo(GameObject toHere)
    {
        if (selected != null)       //if I have a piece selected
        {
            DragPiece temp = selected.GetComponent<DragPiece>();

            if (toHere.transform.childCount == 0 && isEligibleTarget(toHere))   //if there isn't already a piece on top of toHere and is a valid placement
            {
                //set the position on top of toHere, and set the selected piece to be the child of toHere
                float toHereHeight = toHere.GetComponent<Renderer>().bounds.size.y / 2.0f;  
                float selectedHeight = selected.GetComponent<Renderer>().bounds.size.y / 2.0f;
                selected.transform.position = toHere.transform.position + new Vector3(0, toHereHeight + selectedHeight, 0);
                selected.transform.SetParent(toHere.transform);
                temp.inStack = false;
                //no pieces should be movable except those above;
                allPiecesNonMovable();

                markStackMovableUp(selected.transform);
            }
            if (toHere.GetComponent<DragPiece>() != null)       //if the toHere is another Piece
            {
                if (toHere.GetComponent<DragPiece>().inStack)   //if toHere is in the stack
                {
                   selected.transform.SetParent(temp.pieceHolder);      //set back into the stack
                   temp.inStack = true;
                }
            }
            deselectPiece();        //no piece should be selected
        }
    }

    public void selectPiece(GameObject piece)
    {
        if (selected == piece)
        {
            deselectPiece();
        }
        else if (selected == null)
        {
            if (piece.GetComponent<DragPiece>().isMovable)
            {
                selected = piece;
                materialOfSelected = selected.GetComponent<MeshRenderer>().material;
                selected.GetComponent<MeshRenderer>().material = selectedMaterial;
            }
        }
        else
        {
            moveSelectedPieceTo(piece);
        }
        
    }
    bool isEligibleTarget(GameObject toHere)
    {
        Debug.Log("checking eligiblity");
        if(selected.GetComponent<DragPiece>().inStack) {
            Debug.Log("selected is from stack");
            if(toHere.tag != "boardSquare")
            {
                Debug.Log("moving from stack to piece is not allowed");
                return false;   //a move from the stack to another piece is not allowed
            }
            return true;   
        }
        
        if (toHere.transform.position.x == selected.transform.position.x ^ toHere.transform.position.z == selected.transform.position.z)
        {
            Debug.Log("target and piece are in the same row or column");
            return true;
        }
        return false;
    }
    public void deselectPiece()
    {
        if(selected != null)
        {
            selected.GetComponent<MeshRenderer>().material = materialOfSelected;
            selected = null;
        }
    }



    public void nextTurn()
    {
        if(currentTurn == turn.round)
        {
            currentTurn = turn.square;
            playersPieces = roundPieces;
            markMovable();
        }
        else if(currentTurn == turn.square)
        {
            currentTurn = turn.round;
            playersPieces = squarePieces;
            markMovable();
        }
    }
    void markMovable()
    {
        
        foreach(GameObject piece in playersPieces)
        {
            DragPiece thisPiece = piece.GetComponent<DragPiece>();
            if (piece.transform.childCount == 0)
            {
                Debug.Log("foundSomePieces");
                markStackMovableDown(piece.transform);
            }
            
        }
    }

    void markStackMovableDown(Transform top)
    {
        top.GetComponent<DragPiece>().isMovable = true;
        if(top.parent.GetComponent<DragPiece>() != null)
        {
            markStackMovableDown(top.parent);
        }
    }
    void markStackMovableUp(Transform bottom)
    {
        
        if (bottom.childCount > 0)
        {
            bottom.GetChild(0).GetComponent<DragPiece>().isMovable = true;
            markStackMovableUp(bottom.GetChild(0));
        }
    }

    void allPiecesNonMovable()
    {
        foreach(DragPiece data in squareData)
        {
            data.isMovable = false;
        }
        foreach (DragPiece data in roundData)
        {
            data.isMovable = false;
        }
    }
}
