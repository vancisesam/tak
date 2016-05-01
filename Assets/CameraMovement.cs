using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
    public float speed;
    public float minVert;
    public float maxVert;
    // Use this for initialization
    public GameObject centerOfRotation;
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.RightArrow))
        {

            transform.Rotate(Vector3.up * Time.deltaTime*-speed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * speed);
        }
        
        
    }
}
