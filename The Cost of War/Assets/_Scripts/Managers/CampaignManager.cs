using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    public int Campaign1Budget;
    public int Campaign2Budget;
    public int Campaign3Budget;

    public int currentBudget;
    public Text budgetUI;

    public GameObject Campaign1;
    public GameObject Campaign2;
    public GameObject Campaign3;

    public GameObject ControlledTerritories;
        
    private GameObject _cmp1;
    private GameObject _cmp2;
    private GameObject _cmp3;

    private int i = 0;
    private IslandManager islandManager;

    private void Awake()
    {
        _cmp1 = Campaign1.transform.GetChild(0).gameObject;
        _cmp2 = Campaign2.transform.GetChild(0).gameObject;
        _cmp3 = Campaign3.transform.GetChild(0).gameObject;

        islandManager = this.GetComponent<IslandManager>();

        SetCampaignBudget();
        UpdateBudgetUI();
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

    private void SetCampaignBudget()
    {
        if (islandManager.GetCMP3())
        {
            //win scenario
            return;
        }
        if (islandManager.GetCMP2())
        {
            currentBudget = DataStore.budget + Campaign3Budget;
            return;
        }
        if (islandManager.GetCMP1())
        {
            currentBudget = DataStore.budget + Campaign2Budget;
        }
        else if (DataStore.budget == 0) currentBudget = Campaign1Budget;
        else currentBudget = DataStore.budget;
    }

    public void CompleteCampaign1()
    {
        ControlledTerritories.transform.position = _cmp1.transform.position;
        Destroy(_cmp1);
        Destroy(Campaign1);

        currentBudget = Campaign2Budget;
        UpdateBudgetUI();
    }

    public void CompleteCampaign2()
    {
        ControlledTerritories.transform.position = _cmp2.transform.position;
        Destroy(_cmp2);
        Destroy(Campaign2);

        currentBudget = Campaign3Budget;
        UpdateBudgetUI();
    }

    public void CompleteCampaign3()
    {
        ControlledTerritories.transform.position = _cmp3.transform.position;
        Destroy(_cmp3);
        Destroy(Campaign3);
    }

    private void UpdateBudgetUI()
    {
        budgetUI.text = "$ " + currentBudget.ToString();
    }

    public bool Deduct(int amount)
    {
        int temp = currentBudget;
        if (temp - amount >= 0)
        {
            currentBudget -= amount;
            UpdateBudgetUI();
            DataStore.budget = currentBudget;
            return true;
        }
        else return false;
    }
}
