using UnityEngine;
using System.Collections;
using System.Linq;
//using UnityEditor;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private GameObject selected = null;
    public Material selectedMaterial;
    private Material materialOfSelected;
    public GameObject turnText;
    public Text errorText;
    public enum direction { vertical, horizontal, neither};
    private enum directionSign { positive, negative, neither};
    public direction currentDirection = direction.neither;
    private directionSign currentDirectionSign;

    public enum turn { round, square};
    public turn currentTurn=turn.round;
    GameObject[] playersPieces;
    GameObject[] squarePieces;
    GameObject[] roundPieces;
    DragPiece[] squareData;
    DragPiece[] roundData;
    // Use this for initialization
    void Start () {
        roundPieces  = GameObject.FindGameObjectsWithTag("roundPiece");
        squarePieces = GameObject.FindGameObjectsWithTag("squarePiece");
        int i = 0;
        roundData  = new DragPiece[roundPieces.Length];
        squareData = new DragPiece[squarePieces.Length];
        foreach (GameObject piece in roundPieces)
        {
            roundData[i] = roundPieces[i].GetComponent<DragPiece>();
            squareData[i] = squarePieces[i].GetComponent<DragPiece>();
            i++;
        }
    }

    public void moveSelectedPieceTo(GameObject toHere)
    {
        

        if (selected != null)       //if I have a piece selected
        {
            //Undo.RecordObject(selected, "Undomove");
            DragPiece temp = selected.GetComponent<DragPiece>();
            
            if (toHere.transform.childCount == 0 && isEligibleTarget(toHere))   //if there isn't already a piece on top of toHere and is a valid placement
            {
                //set the position on top of toHere, and set the selected piece to be the child of toHere
                float toHereHeight = toHere.GetComponent<Renderer>().bounds.size.y / 2.0f;
                float selectedHeight = selected.GetComponent<Renderer>().bounds.size.y / 2.0f;
                
                selected.transform.position = toHere.transform.position + new Vector3(0, toHereHeight + selectedHeight, 0);
                selected.transform.SetParent(toHere.transform);
                selected.transform.localPosition = new Vector3(0.0f, selected.transform.localPosition.y, 0.0f);
                temp.inStack = false;
                //no pieces should be movable except those above;
                allPiecesNonMovable();

                markStackMovableUp(selected.transform);
            }

            if (toHere.GetComponent<DragPiece>() != null)            //if the toHere is another Piece
            {                                                        
                if (toHere.GetComponent<DragPiece>().inStack)        //if toHere is in the stack
                {
                   selected.transform.SetParent(temp.pieceHolder);   //set back into the stack
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
            DragPiece selectedData = selected.GetComponent<DragPiece>();
            if (selectedData.inStack && !selectedData.isCap)
            {
                if (selectedData.isWall)
                {
                    selectedData.crushWall();
                    deselectPiece();
                }
                else
                {
                    selectedData.makeWall();
                }
            }
            else
            {
                deselectPiece();
            }
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
        Vector3 targetPos = toHere.transform.position;
        Vector3 selectedPos = selected.transform.position;
        //Debug.Log("checking eligiblity");
        if (toHere.tag != "boardSquare")
        { //check if you are moving onto another piece
            DragPiece toHereData = toHere.GetComponent<DragPiece>();
            if (toHereData.isCap)
            {
                setErrorText("You cannot move onto a Cap Stone");
                return false;
            }
            else if (toHereData.isWall)   //cannot move onto a wall
            {
                if (selected.GetComponent<DragPiece>().isCap)
                {
                    toHereData.crushWall();
                    
                    
                    return true;
                }
                setErrorText("You cannot move onto a wall");
                return false;
            }
        }
        if (selected.GetComponent<DragPiece>().inStack) {
            Debug.Log("selected is from stack");
            if (toHere.tag != "boardSquare")
            {
                setErrorText("Moving a piece from stack on top of another piece is not allowed");
                return false;   //a move from the stack to another piece is not allowed
            }
            return true;
        }
        
        else if (targetPos.x == selectedPos.x)
        {
            if (targetPos.z == selectedPos.z)   //the target and the selected are in the same position
            {
                setErrorText("Cannot move to the same position");
                return false;
            }
            else if (currentDirection == direction.horizontal)
            {
                setErrorText("You can only move horizontally");
                return false;
            }
            else if (targetPos.z == selectedPos.z + 1.0f || targetPos.z == selectedPos.z - 1.0f)
            {
                switch (currentDirectionSign)
                {
                    case directionSign.neither:
                        currentDirectionSign = (targetPos.z == selectedPos.z + 1.0f) ? directionSign.positive : directionSign.negative;
                        currentDirection = direction.vertical;
                        return true;
                    case directionSign.negative:
                        return (targetPos.z == selectedPos.z - 1.0f);
                    case directionSign.positive:
                        return (targetPos.z == selectedPos.z + 1.0f);
                }               
                
            }
            setErrorText("You can only move one square at a time");
            return false;
        }
        else if (targetPos.z == selectedPos.z)
        {
            if (currentDirection == direction.vertical)
            {
                setErrorText("You can only move vertically");
                return false;
            }
            else if (targetPos.x == selectedPos.x + 1.0f || targetPos.x == selectedPos.x - 1.0f)
            {
                switch (currentDirectionSign)
                {
                    case directionSign.neither:
                        currentDirectionSign = (targetPos.x == selectedPos.x + 1.0f) ? directionSign.positive : directionSign.negative;
                        currentDirection = direction.horizontal;
                        return true;
                    case directionSign.negative:
                        return (targetPos.x == selectedPos.x - 1.0f);
                    case directionSign.positive:
                        return (targetPos.x == selectedPos.x + 1.0f);
                }
            }
            setErrorText("You can only move one square at a time");
            return false;

        }
        setErrorText("You cannot move diagonally");
        return false;
    }
    public void deselectPiece()
    {
        if(selected != null)
        {
            DragPiece selectedData = selected.GetComponent<DragPiece>();
            if (selectedData.inStack)
            {
                selectedData.crushWall();
            }
            selected.GetComponent<MeshRenderer>().material = materialOfSelected;
            selected = null;
        }
    }



    public void nextTurn() // proceeds to the next players turn
    {
        currentDirection = direction.neither;
        currentDirectionSign = directionSign.neither;
        allPiecesNonMovable();
        if(currentTurn == turn.round)
        {
            currentTurn = turn.square;
            turnText.GetComponent<Text>().text = "Square Turn";
            playersPieces = squarePieces;
            markMovable();
        }
        else if(currentTurn == turn.square)
        {
            currentTurn = turn.round;
            turnText.GetComponent<Text>().text = "Round Turn";
            playersPieces = roundPieces;
            markMovable();
        }
    }

    public void undomove(GameObject toHere) 
    {
        
      
        
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

    void setErrorText(string asdf)
    {
        errorText.text = asdf;
        StartCoroutine(clearErrorText());
    }

    IEnumerator clearErrorText()
    {
        yield return new WaitForSeconds(2.2f);
        errorText.text = "";
    }
}
