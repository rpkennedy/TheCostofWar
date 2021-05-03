using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    private float edgeSize = 70; //number of pixels from the wall
    private float moveAmount = 100; //number of pixels to move by

    private float fastEdgeSize = 35;
    private float fastMoveAmount = 250;

    [Header("Camera Boundaries")]
    private float rightCap = 0;
    private float leftCap = -318;

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
