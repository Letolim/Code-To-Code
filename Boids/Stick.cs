using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stick : MonoBehaviour
{
    public GameObject player;
    public Vector3 dir;
    private Vector3 initialPlayerPosition;
    private Vector3 initialPosition;
    private Vector3 initialMousePosition;
    public bool dragging = false;

    private RectTransform uITransform;

    // Start is called before the first frame update
    void Start()
    {
        uITransform = this.GetComponent<RectTransform>();
        initialPosition = uITransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!dragging)
            Spring();
    }

    private void OnMouseDrag()
    {
        Drag();
    }

    public void MouseUp()
    {
        dragging = false;
    }


    public void MouseDown()
    {
        initialMousePosition = Input.mousePosition;
        dragging = true;
    }

    public void Spring()
    {
        if(uITransform.position.x != initialPosition.x || uITransform.position.y != initialPosition.y)
            uITransform.position = new Vector3(Mathf.Lerp(uITransform.position.x, initialPosition.x,Time.deltaTime * 5f), Mathf.Lerp(uITransform.position.y, initialPosition.y, Time.deltaTime * 5f),0);
        dir = (uITransform.position - initialPosition).normalized;

    }

    public void Drag()
    {
        Vector3 newPosition = Input.mousePosition;
        newPosition.z = 0;
        if (Vector3.Distance(uITransform.position, initialPosition) < 55f)
        { 
            uITransform.position = newPosition;
            //currentPosition = newPosition;
        }else if(Vector3.Distance(newPosition, initialPosition) < 55f && newPosition != uITransform.position)
            uITransform.position = newPosition;

        dir = (uITransform.position - initialPosition).normalized;
    }

}
