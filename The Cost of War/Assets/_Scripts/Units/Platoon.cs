using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Platoon : Unit
{
    private UnitManager unitManager;
    [Header("Unique Attributes")]
    public bool hasExtraDamage = false;
    public bool hasExtraMove = false;

    private List<SpriteRenderer> sprites;
    private Transform landingCraft;
    
    bool beached = false;

    private void Awake()
    {
        sprites = new List<SpriteRenderer>();

        sprites.Add(this.transform.GetChild(0).GetComponent<SpriteRenderer>());
        sprites.Add(this.transform.GetChild(1).GetComponent<SpriteRenderer>());
        sprites.Add(this.transform.GetChild(2).GetComponent<SpriteRenderer>());

        landingCraft = this.transform.GetChild(3);

        sprites.Add(landingCraft.GetChild(0).GetComponent<SpriteRenderer>());
        sprites.Add(landingCraft.GetChild(1).GetComponent<SpriteRenderer>());
        sprites.Add(landingCraft.GetChild(2).GetComponent<SpriteRenderer>());
        sprites.Add(landingCraft.GetChild(3).GetComponent<SpriteRenderer>());

        NewTurn();
        SetHealth();        
    }

    public void UpgradeAttack()
    {
        damage++;
        hasExtraDamage = true;
    }

    public void UpgradeMoveTiles()
    {
        moveTiles += 2;
        hasExtraMove = true;
    }

    public bool IsBeached()
    {
        return beached;
    }

    public void BeachLandingCraft(Cell cell)
    {
        if (beached) return;
        beached = true;
        landingCraft.parent = null;
        landingCraft.position = cell.transform.position - new Vector3(0, 30);
    }

    public void EnableSprites()
    {
        foreach(SpriteRenderer renderer in sprites)
        {
            renderer.enabled = true;
        }
    }
    public void HideSprites()
    {
        foreach (SpriteRenderer renderer in sprites)
        {
            renderer.enabled = false;
        }
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
        unitManager.embarkedVitals.Remove(this.gameObject);
        unitManager.unitsEmbarked.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
