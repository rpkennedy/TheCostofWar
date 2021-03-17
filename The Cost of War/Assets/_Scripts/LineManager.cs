using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _lines;
    private IslandManager _islandManager;

    private void Awake()
    {
        _islandManager = this.gameObject.GetComponent<IslandManager>();

        foreach(GameObject go in _lines)
        {
            go.SetActive(false);
        }
        _lines[0].SetActive(true);
    }

    public void ActivateLine(int line)
    {
        _lines[line].SetActive(true);
    }

    public void CheckLines()
    {
        //Campaign 1
        if (_islandManager.Controls(1))
        {
            _lines[1].SetActive(true);
            _lines[2].SetActive(true);
        }

        //Campaign 2
        if(_islandManager.Controls(2) && !_islandManager.Controls(3))
        {
            _lines[3].SetActive(true);
        }
        if (!_islandManager.Controls(2) && _islandManager.Controls(3))
        {
            _lines[3].SetActive(true);
        }
        if(_islandManager.Controls(2) && _islandManager.Controls(3))
        {
            _lines[4].SetActive(true);
            _lines[5].SetActive(true);
            _lines[6].SetActive(true);
        }

        //Campaign 3
        if (_islandManager.Controls(4) && !_islandManager.Controls(5))
        {
            _lines[7].SetActive(true);
            _lines[9].SetActive(true);
        }
        if (!_islandManager.Controls(4) && _islandManager.Controls(5))
        {
            _lines[8].SetActive(true);
            _lines[9].SetActive(true);
        }
        if (_islandManager.Controls(4) && _islandManager.Controls(5))
        {
            _lines[7].SetActive(true);
            _lines[8].SetActive(true);
            _lines[9].SetActive(true);
        }
        if (_islandManager.Controls(4) && _islandManager.Controls(6) && !_islandManager.Controls(5))
        {
            _lines[8].SetActive(true);
        }
        if (!_islandManager.Controls(4) && _islandManager.Controls(6) && _islandManager.Controls(5))
        {
            _lines[7].SetActive(true);
        }
    }
}
