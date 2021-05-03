using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Enemy
{
    private Operation op;

    private void Awake()
    {
        type = EnemyType.soldier;
        SetHealth();
        op = GameObject.FindGameObjectWithTag("Operation").GetComponent<Operation>();
    }
    
    public void EvalauteTurn()
    {
        List<Cell> neighborCells = op.GetNeighborCells(homeCell);

        foreach (Cell evalCell in neighborCells)
        {
            if (evalCell.hasUnit)
            {
                if (homeCell.isSmoked || evalCell.isSmoked) return;
                if (evalCell.unit == null)
                {
                    evalCell.hasUnit = false;
                }
                if (evalCell.unit.GetComponent<Unit>().type == UnitType.Land)
                {
                    if (evalCell.unit.name.Contains("Platoon"))
                    {
                        evalCell.unit.GetComponent<Platoon>().TakeDamage(damage);
                    }
                    Debug.Log("Soldier attacked " + evalCell.unit.name + " for " + damage + " damage");
                    return;
                }
            }
        }
    }

    /*
    private void ConfigAttackArea()
    {
        attackArea.Add(new Vector2(0, 1));       //up
        attackArea.Add(new Vector2(-1, 1));      //up left
        attackArea.Add(new Vector2(-1, 0));      //left
        attackArea.Add(new Vector2(-1, -1));     //down left
        attackArea.Add(new Vector2(0, -1));      //down
        attackArea.Add(new Vector2(1, -1));      //down right
        attackArea.Add(new Vector2(1, 0));       //right
        attackArea.Add(new Vector2(1, 1));       //up right
    }
    */
}
