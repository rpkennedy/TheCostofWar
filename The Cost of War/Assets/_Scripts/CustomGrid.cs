using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    private Operation operation;

    public GameObject button;
    public GameObject canvas;

    public int width;
    public int height;
    public float _cellSize;
    private int[,] gridArray;

    [Header("For Button placement")]
    public int canvasWidth;
    public int canvasHeight;

    private void Awake()
    {
        operation = GameObject.FindGameObjectWithTag("Operation").GetComponent<Operation>();
        Debug.Log(operation.gameObject.name);
        operation.NewCellArray(width, height);
        CustomGridConstruct(width, height, _cellSize);
        operation.SetGrid(this);
    }
    
    public void CustomGridConstruct(int width, int height, float cellSize)
    {
        gridArray = new int[this.width, this.height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                GameObject tile = Instantiate(button, canvas.transform);
                tile.transform.localPosition = GetWorldPosition(x, y);
                RaycastHit hit;
                if(Physics.Raycast(GetWorldPosition(x,y), -Vector3.forward, out hit))
                {
                    string name = hit.collider.gameObject.name;
                    Cell cell = tile.GetComponent<Cell>();
                    
                    if (name == "Island")
                    {
                        tile.name = "Island Button (" + x + ", " + y + ")";
                        cell.type = CellType.land;
                    }
                    if (name == "Ocean")
                    {
                        tile.name = "Ocean Button (" + x + ", " + y + ")";
                        cell.type = CellType.water;
                    }
                    if (name == "SafeZone")
                    {
                        tile.name = "SafeZone Button (" + x + ", " + y + ")";
                        cell.type = CellType.safeZone;
                    }
                    if(name == "Obstacle")
                    {
                        tile.name = "Obstacle Button (" + x + ", " + y + ")";
                        cell.type = CellType.obstacle;
                    }
                    cell.SetX(x);
                    cell.SetY(y);

                    operation.SetCell(cell);
                }
            }
        }        
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize - new Vector3(canvasWidth/2, canvasHeight/2 ); 
    }
}
