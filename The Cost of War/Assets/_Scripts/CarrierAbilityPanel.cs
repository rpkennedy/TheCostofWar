using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CarrierAbilityPanel : MonoBehaviour
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

    public void InitiateDeploy()
    {
        op.pathfinder.refCell = op.pathfinder.startCell;
        op.pathfinder.startCell = null;
        this.transform.GetChild(0).SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        Destroy(this.gameObject, 0.1f);
    }
}
