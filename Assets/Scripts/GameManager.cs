using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private GameObject selected = null;
    public Material selectedMaterial;
    private Material materialOfSelected;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)){
            //deselectPiece();
        }
	}

    public void movePiece(GameObject toHere)
    {
        if (selected != null)
        {
            DragPiece temp = selected.GetComponent<DragPiece>();
            if (toHere.transform.childCount == 0)
            {
                float toHereHeight = toHere.GetComponent<Renderer>().bounds.size.y / 2.0f;
                float selectedHeight = selected.GetComponent<Renderer>().bounds.size.y / 2.0f;
                selected.transform.position = toHere.transform.position + new Vector3(0, toHereHeight + selectedHeight, 0);
                selected.transform.SetParent(toHere.transform);
                temp.inStack = false;
            }
            if (toHere.GetComponent<DragPiece>() != null)
            {
                if (toHere.GetComponent<DragPiece>().inStack)
                {
                   selected.transform.SetParent(temp.pieceHolder);
                   temp.inStack = true;
                }
            }
            deselectPiece();
        }
    }

    public void selectPiece(GameObject piece)
    {
        if(selected == piece)
        {
            deselectPiece();
        }
        else if(selected == null){
            selected = piece;
            materialOfSelected = selected.GetComponent<MeshRenderer>().material;
            selected.GetComponent<MeshRenderer>().material = selectedMaterial;
        }
        else
        {
            movePiece(piece);
        }
        
    }
    public void deselectPiece()
    {
        if(selected != null)
        {
            selected.GetComponent<MeshRenderer>().material = materialOfSelected;
            selected = null;
        }
    }

}
