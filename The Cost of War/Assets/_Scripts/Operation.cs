using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operation : MonoBehaviour
{
    public int islandID;

    public Pathfinding pathfinder;
    public Cell[,] cells;
    private UnitManager units;
    private Deployer deployer;
    public CustomGrid grid;

    [Header("Enemy Positions")]
    public List<Vector2> soldierCoords;
    public List<Vector2> shipCoords;
    public List<Vector2> aaCoords;

    public List<Enemy> enemies;
    private List<Soldier> soldiers;
    private List<Ship> ships;
    private List<AA> aas;

    [Header("Enemy Prefabs")]
    public GameObject soldier;
    public GameObject ship;
    public GameObject aa;

    [Header("Logic")]
    public bool moveInProgress = false;
    public bool bombing = false;

    private void Start()
    {
        units = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitManager>();

        PlaceVessels();

        soldiers = new List<Soldier>();
        ships = new List<Ship>();
        aas = new List<AA>();

        PlaceEnemies();
        CheckAllFog();

        Invoke("CheckEndgame", 20);
    }

    private void CheckEndgame()
    {
        if(enemies.Count == 0)
        {
            Victory();
        } 
        if(units.embarkedVitals.Count == 0)
        {
            Defeat();
        }

        Invoke("CheckEndgame", 1);
    }

    private void PlaceEnemies()
    {
        for (int i = 0; i < soldierCoords.Count; i++)
        {
            GameObject enemy = Instantiate(soldier);
            cells[(int)soldierCoords[i].x, (int)soldierCoords[i].y].AddEnemy(enemy);
            soldiers.Add(enemy.GetComponent<Soldier>());
            enemies.Add(enemy.GetComponent<Enemy>());
            enemy.GetComponent<Soldier>().SetCell(cells[(int)soldierCoords[i].x, (int)soldierCoords[i].y]);
        }
        for (int i = 0; i < shipCoords.Count; i++)
        {
            GameObject enemy = Instantiate(ship);
            cells[(int)shipCoords[i].x, (int)shipCoords[i].y].AddEnemy(enemy);
            ships.Add(enemy.GetComponent<Ship>());
            enemies.Add(enemy.GetComponent<Enemy>());
            enemy.GetComponent<Ship>().SetCell(cells[(int)shipCoords[i].x, (int)shipCoords[i].y]);
        }
        for (int i = 0; i < aaCoords.Count; i++)
        {
            GameObject enemy = Instantiate(aa);
            cells[(int)aaCoords[i].x, (int)aaCoords[i].y].AddEnemy(enemy);
            aas.Add(enemy.GetComponent<AA>());
            enemies.Add(enemy.GetComponent<Enemy>());
            enemy.GetComponent<AA>().SetCell(cells[(int)aaCoords[i].x, (int)aaCoords[i].y]);
        }
    }

    public void EndPlayerTurn()
    {
        CheckAllFog();  //smoke clearing logic in here too
        Invoke("EnemyEvaluations", 1);
        Invoke("ReplenishPlayerAP", 2);
    }

    private void EnemyEvaluations()
    {
        foreach(Soldier soldier in soldiers)
        {
            soldier.EvalauteTurn();
        }
        foreach(Ship ship in ships)
        {
            ship.EvalauteTurn();
        }
    }

    public void ReconPlaneFogReveal(int y, ReconPlane recon)
    {
        for(int x = 0; x < grid.width; x++)
        {
            cells[x, y].DisableFogRecon();
            if (cells[x, y].hasEnemy)
            {
                if(cells[x, y].enemy.GetComponent<Enemy>().type == EnemyType.AA)
                {
                    recon.TakeDamage(cells[x, y].enemy.GetComponent<Enemy>().damage);
                }
            }
        }
    }

    public void CheckAA(int y, Bomber bomber)
    {
        for (int x = 0; x < grid.width; x++)
        {
            if (cells[x, y].hasEnemy)
            {
                if (cells[x, y].enemy.GetComponent<Enemy>().type == EnemyType.AA)
                {
                    bomber.TakeDamage(cells[x, y].enemy.GetComponent<Enemy>().damage);
                }
            }
        }
    }

    public void CheckAllFog()
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                cells[x, y].CheckFogLogic();
                if (cells[x, y].isSmoked) cells[x, y].SmokeClearing();
            }
        }
    }

    private void ReplenishPlayerAP()
    {
        foreach (GameObject go in units.vesselsEmbarked)
        {
            if (go.name.Contains("Carrier"))
            {
                go.GetComponent<Carrier>().NewTurn();
            }
            if (go.name.Contains("Battleship"))
            {
                go.GetComponent<Battleship>().NewTurn();
            }
        }

        foreach (GameObject go in units.unitsEmbarked)
        {
            if (go.name.Contains("Platoon"))
            {
                go.GetComponent<Platoon>().NewTurn();
            }
        }
    }

    public void SetDeployer(Deployer newDeployer)
    {
        deployer = newDeployer;
    }

    public void SetBomberTarget(Cell target)
    {
        deployer.DeployBomber(target);
    }

    public void SetGrid(CustomGrid newGrid)
    {
        grid = newGrid;
    }

    public void NewCellArray(int x, int y)
    {
        cells = new Cell[x,y];
    }

    public void SetCell(Cell cell)
    {
        cells[cell.GetX(), cell.GetY()] = cell;
    }

    public Cell GetCell(int x, int y)
    {
        Cell cell = cells[x, y];
        Debug.Log("Cell trying to return: " + cell.gameObject.name);
        return cell;
    }
    
    public Cell GetCell(Vector3 pos)
    {
        int x = 0;
        int y = (int)((pos.y +480) / 60);

        if(pos.x >= 0)
        {
            x = (int)grid.width / 2 + (int)(pos.x / 60);
        }
        else if (pos.x < 0)
        {
            x = (int)grid.width / 2 - (int)(pos.x / 60);
        }
        Debug.Log("xCoord: " + x);
        Debug.Log("yCoord: " + y);
        return cells[x, y];
    }

    public Cell GetCellAbove(Cell cell)
    {
        return cells[cell.GetX(), cell.GetY() + 1];
    }
    public Cell GetCellBelow(Cell cell)
    {
        return cells[cell.GetX(), cell.GetY() - 1];
    }
    public Cell GetCellRight(Cell cell)
    {
        return cells[cell.GetX() + 1, cell.GetY()];
    }
    public Cell GetCellLeft(Cell cell)
    {
        return cells[cell.GetX() - 1, cell.GetY()];
    }

    public List<Cell> GetNeighborCells(Cell startCell)
    {
        List<Cell> neighborCells = new List<Cell>();
        Cell aboveCell;
        Cell belowCell;
        
        if(startCell.GetY() < grid.height-1)
        {
            aboveCell = GetCellAbove(startCell);
            neighborCells.Add(aboveCell);               //top cells
            if(startCell.GetX() < grid.width-1) neighborCells.Add(GetCellRight(aboveCell));
            if (startCell.GetX() > 0) neighborCells.Add(GetCellLeft(aboveCell));
            
        }

        if (startCell.GetX() > 0) neighborCells.Add(GetCellLeft(startCell));  //mid cells
        if (startCell.GetX() < grid.width-1) neighborCells.Add(GetCellRight(startCell));

        if(startCell.GetY() > 0)
        {
            belowCell = GetCellBelow(startCell);
            neighborCells.Add(belowCell);               //below cells
            if (startCell.GetX() > 0) neighborCells.Add(GetCellLeft(belowCell));
            if (startCell.GetX() < grid.width-1) neighborCells.Add(GetCellRight(belowCell));
        }

        return neighborCells;
    }

    public List<Cell> GetPlusNeighbors(Cell startCell)
    {
        List<Cell> neighborCells = new List<Cell>();

        if (startCell.GetY() < grid.height - 1) neighborCells.Add(GetCellAbove(startCell));
        if (startCell.GetY() > 0) neighborCells.Add(GetCellBelow(startCell));
        if (startCell.GetX() < grid.width - 1) neighborCells.Add(GetCellRight(startCell));
        if (startCell.GetX() > 0) neighborCells.Add(GetCellLeft(startCell));

        return neighborCells;
    }

    public void PlaceVessels()
    {
        int plusOffset = 1;
        int minusOffset = 0;
        for (int i = 0; i < units.vesselsEmbarked.Count; i++)
        {
            units.vesselsEmbarked[i].GetComponent<SpriteRenderer>().enabled = true;
            if(i%2 == 0)
            {
                cells[(int)(grid.width / 2) - minusOffset, 0].SetUnit(units.vesselsEmbarked[i]);
                minusOffset++;
            }
            else
            {
                cells[(int)(grid.width / 2) + plusOffset, 0].SetUnit(units.vesselsEmbarked[i]);
                plusOffset++;
            }
        }
    }

    public void Victory()
    {
        units.Victory();
    }
    public void Withdrawal()
    {
        units.WithdrawalForces();
    }
    public void Defeat()
    {
        units.Defeat();
    }
}
