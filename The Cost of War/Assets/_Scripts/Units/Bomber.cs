using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Unit
{
    private UnitManager unitManager;

    [Header("Unique Attributes")]
    public bool hasExplosiveOrdinance = false;
    public bool hasExtraOrdinance = false;

    bool flying = false;

    public int turnsUntilCooldown;
    private float distanceTraveled;
    private Cell target;

    public Cell aboveCell;
    public float distPerUpdate;


    private void FixedUpdate()
    {
        if (flying)
        {
            this.gameObject.transform.position += new Vector3(0, distPerUpdate);
            distanceTraveled += distPerUpdate;

            if (aboveCell == null)
            {
                Invoke("StopFlight", 1);
                return;
            }

            if (distanceTraveled >= aboveCell.op.grid._cellSize)
            {
                if (distanceTraveled == aboveCell.op.grid._cellSize)
                {
                    if (aboveCell.GetY() < aboveCell.op.grid.height-1)
                    {
                        aboveCell = aboveCell.op.GetCellAbove(aboveCell);
                        aboveCell.op.CheckAA(aboveCell.GetY(), this);
                    }
                    else
                    {
                        Invoke("StopFlight", 1);
                    }
                    distanceTraveled = 0;
                }
                if (distanceTraveled > aboveCell.op.grid._cellSize)
                {
                    if (aboveCell.op.GetCellAbove(aboveCell) != null)
                    {
                        aboveCell = aboveCell.op.GetCellAbove(aboveCell);
                    }
                    else
                    {
                        Invoke("StopFlight", 1);
                    }
                    distanceTraveled = (int)aboveCell.op.grid._cellSize - distanceTraveled;
                }
                if(aboveCell == target)
                {
                    Debug.Log("BOMBER: Made it over target");
                    if (!hasExplosiveOrdinance)
                    {
                        DropSmokes();
                    }
                    else
                    {
                        DropBombs();
                    }
                }
            }
        }
    }

    private void DropSmokes()
    {
        List<Cell> plusCells = target.op.GetPlusNeighbors(target);
        plusCells.Add(target);

        foreach(Cell cell in plusCells)
        {
            cell.BomberSmoke();
        }
    }
    private void DropBombs()
    {
        List<Cell> plusCells = target.op.GetPlusNeighbors(target);
        plusCells.Add(target);

        foreach (Cell cell in plusCells)
        {
            cell.BomberBomb(damage);
        }
    }

    public void Flyby(Cell bombTarget)
    {
        target = bombTarget;
        flying = true;
        distanceTraveled = 0;
    }

    private void StopFlight()
    {
        flying = false;
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void UpgradeOrdinanceType()
    {
        hasExplosiveOrdinance = true;
    }

    private void SetUnitManager()
    {
        unitManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitManager>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
        Debug.Log(this.gameObject.name + " has " + health + " health left");
    }

    public void Die()
    {
        SetUnitManager();
        unitManager.unitsEmbarked.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
