using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    public GameObject Campaign1;
    public GameObject Campaign2;
    public GameObject Campaign3;

    public GameObject ControlledTerritories;

    public List<Island> cmpIslands1;
    public List<Island> cmpIslands2;
    public List<Island> cmpIslands3;
    
    private GameObject _cmp1;
    private GameObject _cmp2;
    private GameObject _cmp3;

    private int i = 0;

    private void Awake()
    {
        _cmp1 = Campaign1.transform.GetChild(0).gameObject;
        _cmp2 = Campaign2.transform.GetChild(0).gameObject;
        _cmp3 = Campaign3.transform.GetChild(0).gameObject;

        cmpIslands1 = new List<Island>();
        cmpIslands2 = new List<Island>();
        cmpIslands3 = new List<Island>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (i == 0) CompleteCampaign1();
            else if (i == 1) CompleteCampaign2();
            else if (i == 2) CompleteCampaign3();
            i++;
        }
    }    

    public void CompleteCampaign1()
    {
        ControlledTerritories.transform.position = _cmp1.transform.position;
        Destroy(_cmp1);
        Destroy(Campaign1);
    }

    public void CompleteCampaign2()
    {
        ControlledTerritories.transform.position = _cmp2.transform.position;
        Destroy(_cmp2);
        Destroy(Campaign2);
    }

    public void CompleteCampaign3()
    {
        ControlledTerritories.transform.position = _cmp3.transform.position;
        Destroy(_cmp3);
        Destroy(Campaign3);
    }
}
