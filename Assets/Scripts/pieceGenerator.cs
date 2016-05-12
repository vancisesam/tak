using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class pieceGenerator : NetworkBehaviour {
    public GameObject roundPiece;
    public GameObject roundCapStonePiece;
    public GameObject squarePiece;
    public GameObject squareCapStonePiece;
    private GameObject piece;
    private GameObject capStonePiece;
    // Use this for initialization
    void Awake () {
        
    }
    void Start()
    {
        CmdMakePieces();
    }

    [Command]
    void CmdMakePieces()
    {
        
        if (GameObject.FindGameObjectsWithTag("roundPiece").Length == 0)
        {
            piece = roundPiece;
            capStonePiece = roundCapStonePiece;
        }
        else
        {
            piece = squarePiece;
            capStonePiece = squareCapStonePiece;
        }
        for (float j = 0.0f; j < 3; j++)
        {
            for (float i = 0.0f; i < 7; i++)
            {
                GameObject newPiece = Instantiate(piece) as GameObject;
                NetworkServer.Spawn(newPiece);
                newPiece.transform.SetParent(transform);
                newPiece.transform.localPosition = Vector3.zero + new Vector3(0.0f, i * newPiece.GetComponent<Renderer>().bounds.size.y, j);
                newPiece.GetComponent<DragPiece>().inStack = true;
                //NetworkServer.Spawn(newPiece);
            }
        }
        GameObject capStone = Instantiate(capStonePiece) as GameObject;
        capStone.transform.SetParent(transform);
        capStone.transform.localPosition = Vector3.zero + new Vector3(0.0f, .35f, -1.0f);
        capStone.GetComponent<DragPiece>().inStack = true;
        capStone.GetComponent<DragPiece>().isCap = true;
    }
}
