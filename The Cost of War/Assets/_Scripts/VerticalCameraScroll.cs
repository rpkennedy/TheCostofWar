using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalCameraScroll : MonoBehaviour
{
    private float edgeSize = 70; //number of pixels from the wall
    private float moveAmount = 50; //number of pixels to move by

    private float fastEdgeSize = 40;
    private float fastMoveAmount = 150;

    [Header("Camera Boundaries")]
    public float topCap;
    public float bottomCap;

    private Transform _main;

    private void Awake()
    {
        _main = Camera.main.gameObject.transform;
    }
    private void Update()
    {
        if (Input.mousePosition.y > Screen.height - edgeSize)
        {
            if (_main.position.y + moveAmount * Time.deltaTime > topCap) return;
            if (Input.mousePosition.y > Screen.height - fastEdgeSize)
            {
                if (_main.position.y + fastMoveAmount * Time.deltaTime > topCap) return;
                _main.position += new Vector3(0,fastMoveAmount * Time.deltaTime);
            }
            else _main.position += new Vector3(0, moveAmount * Time.deltaTime);
        }
        if (Input.mousePosition.y < edgeSize)
        {
            if (_main.position.y - moveAmount * Time.deltaTime < bottomCap) return;
            if (Input.mousePosition.y < fastEdgeSize)
            {
                if (_main.position.y - fastMoveAmount * Time.deltaTime < bottomCap) return;
                _main.position += new Vector3(0,-fastMoveAmount * Time.deltaTime);
            }
            else _main.position += new Vector3(0,-moveAmount * Time.deltaTime);
        }
    }
}
