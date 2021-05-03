using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    soldier,
    AA,
    ship
}

public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    public int health;
    protected int currentHealth;

    public Cell homeCell;
    public int damage;

    public EnemyType type;
    public int xCoord;
    public int yCoord;

    public void SetHealth()
    {
        currentHealth = health;
    }
    
    public void SetCell(Cell cell)
    {
        homeCell = cell;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
        Debug.Log(this.gameObject.name + " has " + currentHealth + " health left");
    }

    protected void Die()
    {
        homeCell.op.enemies.Remove(this);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        homeCell.hasEnemy = false;
        homeCell.enemy = null;
        Destroy(this.gameObject);
    }
}
