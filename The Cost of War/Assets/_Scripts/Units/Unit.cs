using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public enum UnitType
{
    Land,
    Air,
    Sea
}

public class Unit : MonoBehaviour
{
    [Header("Unit Ability UI Prefabs")]
    public GameObject platoonAbilityPrefab;
    public GameObject carrierAbilityPrefab;
    public GameObject battleshipAbilityPrefab;

    [Header("Unit Attributes")]
    public string unitName;
    public int maxHealth;
    public int health;
    public int damage;
    public int moveTiles;
    public int attackDistance;
    public int actionPoints;

    public int currentAP;

    public int cost;
    public UnitType type;

    private void Awake()
    {
        SetHealth();
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth()
    {
        health = maxHealth;
    }

    public void Heal()
    {
        if(health < maxHealth)
        {
            health++;
        }
    }
    
    public bool VerifyAction()
    {
        if(currentAP > 0)
        {
            currentAP--;
            Debug.Log(this.gameObject.name + " has " + currentAP + " AP left");
            return true;
        }
        else
        {
            Debug.Log(this.gameObject.name + " has " + currentAP + " AP left");
            return false;
        }
    }
    public bool HasAP()
    {
        if (currentAP > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void NewTurn()
    {
        currentAP = actionPoints;
    }

    public void ShowAbilities()
    {
        GameObject go = null;
        if (unitName.Contains("Platoon"))
        {
            go = Instantiate(platoonAbilityPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
        }
        if (unitName.Contains("Carrier"))
        {
            go = Instantiate(carrierAbilityPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
        }
        if (unitName.Contains("Battleship"))
        {
            go = Instantiate(battleshipAbilityPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
        }
        go.transform.position = this.transform.position;
    }
}
    
