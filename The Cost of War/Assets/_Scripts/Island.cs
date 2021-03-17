using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public int id;

    public bool isControlled = false;
    public float scanRadius;

    private IslandManager _islandManager;

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
