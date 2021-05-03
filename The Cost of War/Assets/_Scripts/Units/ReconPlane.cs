using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ReconPlane : Unit
{
    private UnitManager unitManager;

    [Header("Unique Attributes")]
    public bool hasLongerScan = false;
    public bool hasExtraHealth = false;

    public int turnsUntilCooldown;
    private float distanceTraveled;

    public Cell aboveCell;
    public float distPerUpdate;

    bool flying = false;

    void Awake()
    {
        SetUnitManager();
        SetHealth();
    }

    public void Recon()
    {
        flying = true;
        distanceTraveled = 0;
    }

    private void FixedUpdate()
    {
        if (flying)
        {
            this.gameObject.transform.position += new Vector3(0, distPerUpdate);
            distanceTraveled += distPerUpdate;

            if(aboveCell == null)
            {
                Invoke("StopFlight", 1);
            }

            if(distanceTraveled >= aboveCell.op.grid._cellSize)
            {
                if(distanceTraveled == aboveCell.op.grid._cellSize)
                {
                    if(aboveCell.op.GetCellAbove(aboveCell) != null)
                    {
                        aboveCell = aboveCell.op.GetCellAbove(aboveCell);
                        aboveCell.op.ReconPlaneFogReveal(aboveCell.GetY(), this);
                    }
                    else
                    {
                        Invoke("StopFlight", 1);
                    }
                    distanceTraveled = 0;
                }
                if(distanceTraveled > aboveCell.op.grid._cellSize)
                {
                    if (aboveCell.GetY() < aboveCell.op.grid.height - 1)
                    {
                        aboveCell = aboveCell.op.GetCellAbove(aboveCell);
                    }
                    else
                    {
                        Invoke("StopFlight", 1);
                    }
                    distanceTraveled = (int)aboveCell.op.grid._cellSize - distanceTraveled;
                }
            }
        }
    }

    private void StopFlight()
    {
        flying = false;
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void UpgradeHealth()
    {
        maxHealth++;
        SetHealth();
        hasExtraHealth = true;
    }

    public void UpgradeScan()
    {
        hasLongerScan = true;
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
