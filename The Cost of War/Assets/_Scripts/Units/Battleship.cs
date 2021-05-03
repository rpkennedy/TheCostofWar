using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Battleship : Unit
{
    private UnitManager unitManager;
    [Header("Unique Attributes")]
    public bool hasExtraHealth = false;
    public bool hasExtraMove = false;

    private void Awake()
    {
        NewTurn();
        SetHealth();
    }

    public void UpgradeHealth()
    {
        maxHealth++;
        SetHealth();
        hasExtraHealth = true;
    }

    public void UpgradeMoveTiles()
    {
        moveTiles++;
        hasExtraMove = true;
    }

    private void SetUnitManager()
    {
        unitManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitManager>();
        Debug.Log(unitManager.gameObject.name);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
        Debug.Log(this.gameObject.name + " has " + health + " health left");
    }

    private void Die()
    {
        SetUnitManager();
        unitManager.embarkedVitals.Remove(this.gameObject);
        unitManager.vesselsEmbarked.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
