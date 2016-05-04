using UnityEngine;
using System.Collections;

public class pieceGenerator : MonoBehaviour {
    public GameObject piece;
    public GameObject capStonePiece;
	// Use this for initialization
	void Awake () {
        for (float j = 0.0f; j < 3; j++)
        {
            for (float i = 0.0f; i < 7; i++)
            {
                GameObject newPiece = Instantiate(piece) as GameObject;
                newPiece.transform.SetParent(transform);
                newPiece.transform.localPosition = Vector3.zero + new Vector3(0.0f, i * newPiece.GetComponent<Renderer>().bounds.size.y,j);
                newPiece.GetComponent<DragPiece>().inStack = true;
            }
        }
        GameObject capStone = Instantiate(capStonePiece) as GameObject;
        capStone.transform.SetParent(transform);
        capStone.transform.localPosition = Vector3.zero + new Vector3(0.0f, .35f, - 1.0f);
        capStone.GetComponent<DragPiece>().inStack = true;
        capStone.GetComponent<DragPiece>().isCap = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
