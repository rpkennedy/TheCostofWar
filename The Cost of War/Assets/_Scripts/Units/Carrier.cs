using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Carrier : Unit
{
    private UnitManager unitManager;

    public List<Platoon> platoonsStored;
    public List<ReconPlane> reconPlanesStored;
    public List<Bomber> bombersStored;

    [Header("Unique Attributes")]
    public int landUnitMaxCapacity = 3;
    public int airUnitMaxCapacity = 1;

    public int landUnitCapacity;
    public int airUnitCapacity;

    private void Awake()
    {
        platoonsStored = new List<Platoon>();
        reconPlanesStored = new List<ReconPlane>();
        bombersStored = new List<Bomber>();

        landUnitCapacity = landUnitMaxCapacity;
        airUnitCapacity = airUnitMaxCapacity;

        NewTurn();
        SetHealth();
    }
    
    public void UpgradeLandCapacity()
    {
        landUnitCapacity = landUnitMaxCapacity + 1;
    }

    public void UpgradeAirCapacity()
    {
        airUnitCapacity = airUnitCapacity + 1;
    }

    public void StorePlatoon(Platoon plat)
    {
        platoonsStored.Add(plat);
    }
    public void StoreRecon(ReconPlane recon)
    {
        reconPlanesStored.Add(recon);
    }
    public void StoreBomber(Bomber bomber)
    {
        bombersStored.Add(bomber);
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

    private void Die()
    {
        SetUnitManager();
        foreach (Platoon plat in platoonsStored)
        {
            plat.Die();
        }
        foreach(ReconPlane recon in reconPlanesStored)
        {
            if(recon != null)
            {
                recon.Die();
            }            
        }
        foreach(Bomber bomber in bombersStored)
        {
            if(bomber != null)
            {
                bomber.Die();
            }
        }
        unitManager.vesselsEmbarked.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
