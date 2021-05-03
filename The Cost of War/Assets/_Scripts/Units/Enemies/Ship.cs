using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Enemy
{
    private Operation op;

    private void Awake()
    {
        type = EnemyType.ship;
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
                if(evalCell.unit == null)
                {
                    evalCell.hasUnit = false;
                }
                else if (evalCell.unit.GetComponent<Unit>().type != UnitType.Air)
                {
                    if (evalCell.unit.name.Contains("Platoon"))
                    {
                        if(!evalCell.unit.GetComponent<Platoon>().IsBeached()) evalCell.unit.GetComponent<Platoon>().TakeDamage(damage);
                    }
                    if (evalCell.unit.name.Contains("ship")) evalCell.unit.GetComponent<Battleship>().TakeDamage(damage);
                    if (evalCell.unit.name.Contains("Carrier")) evalCell.unit.GetComponent<Carrier>().TakeDamage(damage);
                    
                    Debug.Log("Ship attacked " + evalCell.unit.name + " for " + damage + " damage");
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

        attackArea.Add(new Vector2(0, 2));      //far up
        attackArea.Add(new Vector2(0, -2));     //far down
        attackArea.Add(new Vector2(2, 0));      //far right
        attackArea.Add(new Vector2(2, 0));      //far left
    }
    */
}
