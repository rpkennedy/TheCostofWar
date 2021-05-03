using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipAbilityPanel : MonoBehaviour
{
    private Cell startCell;
    private Operation op;

    private void Awake()
    {
        op = GameObject.FindGameObjectWithTag("Operation").GetComponent<Operation>();
        startCell = op.pathfinder.startCell;
    }

    public void InitiateMovement()
    {
        op.moveInProgress = true;
        op.pathfinder.SetStartCell(startCell);
        Destroy(this.gameObject);
    }

    public void InitiateAttack()
    {
        op.pathfinder.startCell = startCell;
        op.pathfinder.SetAttacking();
        Destroy(this.gameObject);
    }
}
