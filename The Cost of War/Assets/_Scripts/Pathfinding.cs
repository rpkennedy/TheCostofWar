using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Cell startCell;
    public Cell targetCell;

    public Cell refCell;
    public Unit deployUnit;

    private Operation op;

    bool isMoving = false;
    bool attacking = false;

    private void Awake()
    {
        op = GameObject.FindGameObjectWithTag("Operation").GetComponent<Operation>();
    }

    private void Update()
    {
        if (startCell != null && targetCell != null)    //both nodes set
        {
            if (startCell.unit == null) return;
            if (attacking && !op.moveInProgress)    //if we are attacking
            {
                if (targetCell.enemy == null)        //if target cell has no unit to attack, fail
                {
                    Debug.Log("Test 1");
                    PathFail();
                    attacking = false;
                    return;
                }
                if (CalculateDistance() <= startCell.unit.GetComponent<Unit>().attackDistance)
                {
                    if (startCell.unit.GetComponent<Unit>().VerifyAction())
                    {
                        AttackSuccess();    //if has unit and is in range, attack
                        op.CheckAllFog();
                    }
                    else PathFail();
                }
                else
                {
                    Debug.Log("Test 2");    //if out of range, fail
                    PathFail();
                }
                attacking = false;
            }
            else if (op.moveInProgress) //if not attacking, are we moving
            {   //succeed if it is in movement range
                
                if (CalculateDistance() <= startCell.unit.GetComponent<Unit>().moveTiles)
                {
                    if(startCell.unit.GetComponent<Unit>().type != UnitType.Sea)
                    {
                        if (startCell.unit.GetComponent<Unit>().VerifyAction())
                        {
                            PathSuccess();    //if has unit and is in range, attack
                            op.CheckAllFog();
                        }
                        else return;
                    }
                    else if(startCell.unit.GetComponent<Unit>().type == UnitType.Sea && targetCell.type == CellType.water || targetCell.type == CellType.safeZone)
                    {
                        if (startCell.unit.GetComponent<Unit>().VerifyAction())
                        {
                            PathSuccess();   //if has unit and is in range, attack
                            op.CheckAllFog();
                        }
                        else return;
                    }
                    else 
                    {
                        PathFail();
                    }
                }                    
                else
                {
                    Debug.Log("Test 3");    //fail if too far
                    PathFail();
                }
                op.moveInProgress = false;
            }
        }
        if (startCell == null && targetCell != null && deployUnit != null)
        {
            if (CalculateDistanceRef() <= deployUnit.moveTiles)
            {
                if (refCell.unit.GetComponent<Unit>().VerifyAction())
                {
                    PathSuccessRef();
                    op.CheckAllFog();
                    GameObject.FindGameObjectWithTag("Deployer").GetComponent<Deployer>().Success();
                }
                else GameObject.FindGameObjectWithTag("Deployer").GetComponent<Deployer>().End();
            }
            else
            {
                PathFailRef();
                GameObject.FindGameObjectWithTag("Deployer").GetComponent<Deployer>().End();
            }
        }
    }

    public void SetAttacking()
    {
        attacking = true;
        op.moveInProgress = false;
    }

    public void StopAttacking()
    {
        attacking = false;
    }

    public bool GetAttacking()
    {
        return attacking;
    }

    public void SetTargetCell(Cell cell)
    {
        targetCell = cell;
    }

    public void SetStartCell(Cell cell)
    {
        startCell = cell;
    }

    public void StartMoveForDeploy(Unit unit)
    {
        deployUnit = unit;
    }

    private void AttackSuccess()
    {
        Debug.Log(startCell.unit.name + " attacked " + targetCell.enemy.name);
        targetCell.enemy.GetComponent<Enemy>().TakeDamage(startCell.unit.GetComponent<Unit>().damage);
               
        startCell.SetButtonGreen();
        targetCell.SetTargetButtonGreen();

        foreach (Vector2 coord in CalculatePath())
        {
            op.cells[(int)coord.x, (int)coord.y].SetButtonGreen();
        }
        targetCell = null;
    }

    private void PathSuccess()
    {
        targetCell.SetUnit(startCell.unit);
        startCell.unit = null;
        startCell.hasUnit = false;
        Platoon plat = null;

        if (targetCell.unit.name.Contains("Platoon")) //used for landing craft logic
        {
            plat = targetCell.unit.GetComponent<Platoon>();
        }

        Debug.Log("Path Success");

        startCell.SetButtonGreen();
        targetCell.SetTargetButtonGreen();
        
        foreach(Vector2 coord in CalculatePath())
        {
            op.cells[(int)coord.x, (int)coord.y].SetButtonGreen();
            if(op.cells[(int)coord.x, (int)coord.y].type == CellType.land && plat != null)
            {
                plat.BeachLandingCraft(op.cells[(int)coord.x, (int)coord.y]);
                targetCell = null;
                return;
            }
            else if (targetCell.type == CellType.land && plat != null)
            {
                plat.BeachLandingCraft(targetCell);
            }
        }
        targetCell = null;
    }

    private void PathSuccessRef()
    {
        Debug.Log("Success Deploying Unit");
        targetCell.SetUnit(deployUnit.gameObject);

        refCell.SetButtonGreen();
        targetCell.SetTargetButtonGreen();

        foreach (Vector2 coord in CalculatePathRef())
        {
            op.cells[(int)coord.x, (int)coord.y].SetButtonGreen();
        }
        deployUnit = null;
        targetCell = null;
    }

    private void PathFail()
    {
        startCell.SetButtonRed();
        targetCell.SetTargetButtonRed();

        Debug.Log("Path Fail");

        foreach (Vector2 coord in CalculatePath())
        {
            op.cells[(int)coord.x, (int)coord.y].SetButtonRed();
        }
        targetCell = null;
    }

    private void PathFailRef()
    {
        Debug.Log("Failure Deploying Unit");
        refCell.SetButtonRed();
        targetCell.SetTargetButtonRed();

        foreach (Vector2 coord in CalculatePathRef())
        {
            op.cells[(int)coord.x, (int)coord.y].SetButtonRed();
        }
        deployUnit = null;
        targetCell = null;
    }

    public void SetCell(Cell cell)
    {
        if(!isMoving)
        {
            if(deployUnit != null)
            {
                targetCell = cell;
            }
            if (cell.unit == null) return;
            startCell = cell;
            isMoving = true;
            return;
        }
        if (isMoving)
        {
            if (cell == startCell && !op.moveInProgress)
            {
                startCell = null;
                isMoving = false;
                return;
            }
            if (cell.unit != null && !attacking) return;
            targetCell = cell;
            isMoving = false;
        }
        if (attacking && cell.unit != null) targetCell = cell;
    }

    private int CalculateDistance()
    {
        int distance = 0;

        if(startCell.GetX() > targetCell.GetX())    // x distance if starting x is greater
        {
            for(int i = startCell.GetX(); i != targetCell.GetX(); i--)
            {
                distance++;
            }
        }
        if (startCell.GetX() < targetCell.GetX())    // x distance if starting x is lower
        {
            for (int i = startCell.GetX(); i != targetCell.GetX(); i++)
            {
                distance++;
            }
        }

        if (startCell.GetY() > targetCell.GetY())    // y distance if starting y is greater
        {
            for (int i = startCell.GetY(); i != targetCell.GetY(); i--)
            {
                distance++;
            }
        }
        if (startCell.GetY() < targetCell.GetY())    // y distance if starting y is lower
        {
            for (int i = startCell.GetY(); i != targetCell.GetY(); i++)
            {
                distance++;
            }
        }

        Debug.Log("Distance between cells: " + distance);

        return distance;
    }

    public int CalculateDistanceRef()
    {
        int distance = 0;

        if (refCell.GetX() > targetCell.GetX())    // x distance if starting x is greater
        {
            for (int i = refCell.GetX(); i != targetCell.GetX(); i--)
            {
                distance++;
            }
        }
        if (refCell.GetX() < targetCell.GetX())    // x distance if starting x is lower
        {
            for (int i = refCell.GetX(); i != targetCell.GetX(); i++)
            {
                distance++;
            }
        }

        if (refCell.GetY() > targetCell.GetY())    // y distance if starting y is greater
        {
            for (int i = refCell.GetY(); i != targetCell.GetY(); i--)
            {
                distance++;
            }
        }
        if (refCell.GetY() < targetCell.GetY())    // y distance if starting y is lower
        {
            for (int i = refCell.GetY(); i != targetCell.GetY(); i++)
            {
                distance++;
            }
        }

        Debug.Log("Distance between cells: " + distance);

        return distance;
    }


    private List<Vector2> CalculatePath()
    {
        List<Vector2> cellPath = new List<Vector2>();

        if (startCell.GetX() >= targetCell.GetX())    // x distance if starting x is greater
        {
            if (startCell.GetY() >= targetCell.GetY())    // y distance if starting y is greater
            {
                if (startCell.GetY() != targetCell.GetY())
                {
                    for (int y = startCell.GetY(); y != targetCell.GetY(); y--)
                    {
                        cellPath.Add(new Vector2(startCell.GetX(), y));
                    }
                }
                if (startCell.GetX() != targetCell.GetX())
                {
                    for (int x = startCell.GetX() - 1; x != targetCell.GetX(); x--)
                    {
                        cellPath.Add(new Vector2(x, targetCell.GetY()));
                    }
                }
            }
            if (startCell.GetY() < targetCell.GetY())    // y distance if starting y is lower
            {
                if (startCell.GetY() != targetCell.GetY())
                {
                    for (int y = startCell.GetY(); y != targetCell.GetY(); y++)
                    {
                        cellPath.Add(new Vector2(startCell.GetX(), y));
                    }
                }
                if (startCell.GetX() != targetCell.GetX())
                {
                    for (int x = startCell.GetX() - 1; x != targetCell.GetX(); x--)
                    {
                        cellPath.Add(new Vector2(x, targetCell.GetY()));
                    }
                }
            }
        }
        if (startCell.GetX() < targetCell.GetX())    // x distance if starting x is lower
        {
            if (startCell.GetY() >= targetCell.GetY())    // y distance if starting y is greater
            {
                if (startCell.GetY() != targetCell.GetY())
                {
                    for (int y = startCell.GetY(); y != targetCell.GetY(); y--)
                    {
                        cellPath.Add(new Vector2(startCell.GetX(), y));
                    }
                }
                if (startCell.GetX() != targetCell.GetX())
                {
                    for (int x = startCell.GetX() + 1; x != targetCell.GetX(); x++)
                    {
                        cellPath.Add(new Vector2(x, targetCell.GetY()));
                    }
                }
            }
            if (startCell.GetY() < targetCell.GetY())    // y distance if starting y is lower
            {
                if (startCell.GetY() != targetCell.GetY())
                {
                    for (int y = startCell.GetY(); y != targetCell.GetY(); y++)
                    {
                        cellPath.Add(new Vector2(startCell.GetX(), y));
                    }
                }
                if (startCell.GetX() != targetCell.GetX())
                {
                    for (int x = startCell.GetX() + 1; x != targetCell.GetX(); x++)
                    {
                        cellPath.Add(new Vector2(x, targetCell.GetY()));
                    }
                }
            }
        }
        return cellPath;
    }

    private List<Vector2> CalculatePathRef()
    {
        List<Vector2> cellPath = new List<Vector2>();

        if (refCell.GetX() >= targetCell.GetX())    // x distance if starting x is greater
        {
            if (refCell.GetY() >= targetCell.GetY())    // y distance if starting y is greater
            {
                if (refCell.GetY() != targetCell.GetY())
                {
                    for (int y = refCell.GetY(); y != targetCell.GetY(); y--)
                    {
                        cellPath.Add(new Vector2(refCell.GetX(), y));
                    }
                }
                if (refCell.GetX() != targetCell.GetX())
                {
                    for (int x = refCell.GetX() - 1; x != targetCell.GetX(); x--)
                    {
                        cellPath.Add(new Vector2(x, targetCell.GetY()));
                    }
                }
            }
            if (refCell.GetY() < targetCell.GetY())    // y distance if starting y is lower
            {
                if (refCell.GetY() != targetCell.GetY())
                {
                    for (int y = refCell.GetY(); y != targetCell.GetY(); y++)
                    {
                        cellPath.Add(new Vector2(refCell.GetX(), y));
                    }
                }
                if (refCell.GetX() != targetCell.GetX())
                {
                    for (int x = refCell.GetX() - 1; x != targetCell.GetX(); x--)
                    {
                        cellPath.Add(new Vector2(x, targetCell.GetY()));
                    }
                }
            }
        }
        if (refCell.GetX() < targetCell.GetX())    // x distance if starting x is lower
        {
            if (refCell.GetY() >= targetCell.GetY())    // y distance if starting y is greater
            {
                if (refCell.GetY() != targetCell.GetY())
                {
                    for (int y = refCell.GetY(); y != targetCell.GetY(); y--)
                    {
                        cellPath.Add(new Vector2(refCell.GetX(), y));
                    }
                }
                if (refCell.GetX() != targetCell.GetX())
                {
                    for (int x = refCell.GetX() + 1; x != targetCell.GetX(); x++)
                    {
                        cellPath.Add(new Vector2(x, targetCell.GetY()));
                    }
                }
            }
            if (refCell.GetY() < targetCell.GetY())    // y distance if starting y is lower
            {
                if (refCell.GetY() != targetCell.GetY())
                {
                    for (int y = refCell.GetY(); y != targetCell.GetY(); y++)
                    {
                        cellPath.Add(new Vector2(refCell.GetX(), y));
                    }
                }
                if (refCell.GetX() != targetCell.GetX())
                {
                    for (int x = refCell.GetX() + 1; x != targetCell.GetX(); x++)
                    {
                        cellPath.Add(new Vector2(x, targetCell.GetY()));
                    }
                }
            }
        }
        return cellPath;   
    }

    /*
    void FindPath()
    {
        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startCell);

        while(OpenList.Count > 0)
        {
            Node currentNode = OpenList[0];
            for(int i = 1; i < OpenList.Count; i++)
            {
                if(OpenList[i].fCost <= currentNode.fCost && OpenList[i].hCost < currentNode.hCost)
                {
                    currentNode = OpenList[i];
                }
            }
            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            if(currentNode == targetCell)
            {
                GetFinalPath(startCell, targetCell);
            }

            foreach(Node node in grid.GetNeighborNodes(currentNode))
            {
                if (!node.isObstacle || ClosedList.Contains(node)) continue;

                int moveCost = currentNode.gCost + GetManhatDistance(currentNode, node);

                if(moveCost < node.gCost || !OpenList.Contains(node))
                {
                    node.gCost = moveCost;
                    node.hCost = GetManhatDistance(node, targetCell);
                    node.parent = currentNode;

                    if (!OpenList.Contains(node)) OpenList.Add(node);
                }
            }
        }

        void GetFinalPath(Node begin, Node end)
        {
            Debug.Log("Finding path");
            List<Node> FinalPath = new List<Node>();
            Node currentNode = end;

            LineRenderer line = this.gameObject.AddComponent<LineRenderer>();
            List<Vector3> points = new List<Vector3>();
            while(currentNode != begin)
            {
                points.Add(new Vector3(currentNode.x * 60, currentNode.y * 60));
                Debug.Log(currentNode.x + ", " + currentNode.y);
                FinalPath.Add(currentNode);
                currentNode = currentNode.parent;
            }
            FinalPath.Reverse();

            grid.finalPath = FinalPath;
            startCell = null;
            targetCell = null;
        }
    }

    int GetManhatDistance(Node a, Node b)
    {
        int x = Mathf.Abs(a.x - b.x);
        int y = Mathf.Abs(a.y - b.y);

        return x + y;
    }
    */
}
