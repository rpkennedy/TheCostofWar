using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    [Header("AUTOMATIC")]
    public int id;
    public bool isControlled = false;

    private IslandManager _islandManager;

    [Header("MANUAL")]
    public Vector2 reportPosition;

    [Header("0 green; 1 yellow; 2 red")]
    public int difficulty;

    [Header("0 absent; 1 weak; 2 strong")]
    public int aerialThreat;
    public int landThreat;
    public int navalThreat;

    

    private void Awake()
    {
        _islandManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<IslandManager>();
    }

    public void Attack()
    {
        isControlled = true;
        _islandManager.RemoveButton(id);
        _islandManager.CheckLogic();
    }
}
