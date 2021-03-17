using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public float edgeSize; //number of pixels from the wall
    public float moveAmount; //number of pixels to move by

    public float fastEdgeSize;
    public float fastMoveAmount;

    [Header("Camera Boundaries")]
    public float rightCap;
    public float leftCap;

    private Transform _main;

    private void Awake()
    {
        _main = Camera.main.gameObject.transform;
    }
    private void Update()
    {
        if(Input.mousePosition.x > Screen.width - edgeSize)
        {
            if (_main.position.x + moveAmount * Time.deltaTime > rightCap) return;
            if (Input.mousePosition.x > Screen.width - fastEdgeSize)
            {
                if (_main.position.x + fastMoveAmount * Time.deltaTime > rightCap) return;
                _main.position += new Vector3(fastMoveAmount * Time.deltaTime, 0);
            }
            else _main.position += new Vector3(moveAmount * Time.deltaTime, 0);
        }
        if (Input.mousePosition.x < edgeSize)
        {
            if (_main.position.x - moveAmount * Time.deltaTime < leftCap) return;
            if(Input.mousePosition.x < fastEdgeSize)
            {
                if (_main.position.x - fastMoveAmount * Time.deltaTime < leftCap) return;
                _main.position += new Vector3(-fastMoveAmount * Time.deltaTime, 0);
            }
            else _main.position += new Vector3(-moveAmount * Time.deltaTime, 0);
        }
    }
}
