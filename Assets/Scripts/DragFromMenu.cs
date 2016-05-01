using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DragFromMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject flatPiece;

    public float scale;

    private float height;
    private Vector3 originalPosition;
    private Vector3 originalSize;
    private Vector3 point;

    // Use this for initialization
    void Start()
    {
        height = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*void OnMouseDown()
    {
        Debug.Log("Ive been CLicked");
        originalPosition = transform.position;
        Cursor.visible = false;
        originalSize = transform.localScale;
        transform.localScale = originalSize * scale;
    }
    void OnMouseDrag()
    {
        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //point.z = gameObject.transform.position.z;
        gameObject.transform.position = point;
    }
    void OnMouseUp()
    {
        transform.localScale = originalSize;
        Vector3 endPosition = new Vector3(Mathf.Round(point.x), height, Mathf.Round(point.x));
        GameObject newPiece = Instantiate(flatPiece, endPosition, Quaternion.identity) as GameObject;



        transform.position = originalPosition;
    }
    */
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        originalPosition = transform.position;
        Cursor.visible = false;
        originalSize = transform.localScale;
        transform.localScale = originalSize * scale;

    }

    public void OnDrag(PointerEventData eventData)
    {
        point = eventData.position;
        //point.z = gameObject.transform.position.z;
        transform.position = point;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localScale = originalSize;
        Vector3 endPosition = new Vector3(Mathf.Round(point.x), height, Mathf.Round(point.x));
        //GameObject newPiece = Instantiate(flatPiece, endPosition, Quaternion.identity) as GameObject;
        Cursor.visible = true;


        //transform.position = originalPosition;
    }
}
