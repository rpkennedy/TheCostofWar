using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    [SerializeField]
    private List<Island> _islands;
    [SerializeField]
    private List<Button> _buttons;

    private LineManager _lineManager;
    private CampaignManager _campaignManager;

    bool checkCmp1 = false;
    bool checkCmp2 = false;
    bool checkCmp3 = false;

    private void Start()
    {
        _lineManager = this.gameObject.GetComponent<LineManager>();
        _campaignManager = this.gameObject.GetComponent<CampaignManager>();

        for (int i = 0; i < _islands.Count; i++)
        {
            _islands[i].id = i;
            _islands[i].isControlled = false;
        }
        _islands[0].isControlled = true;
        _buttons[0].gameObject.SetActive(true);

        ReadData();
        CheckLogic();
    }    

    private void ReadData()
    {
        for(int i = 0; i < 6; i++)
        {
            _islands[i+1].isControlled = DataStore.islandControl[i];
        }
    }

    public bool GetCMP1()
    {
        return checkCmp1;
    }
    public bool GetCMP2()
    {
        return checkCmp2;
    }
    public bool GetCMP3()
    {
        return checkCmp3;
    }

    public Island GetIsland(int id)
    {
        return _islands[id];
    }    

    public void AttackIsland(int id)
    {
        _islands[id].Attack();
    }

    public bool Controls(int id)
    {
        return _islands[id].isControlled;
    }

    public void RemoveButton(int id)
    {
        id--;
        _buttons[id].interactable = false;
        _buttons[id].targetGraphic = null;
    }

    public void CheckLogic() 
    {
        _lineManager.CheckLines();
        CheckCampaignCompletion();
        CheckButtons();
    }

    private void CheckButtons()
    {
        if (Controls(1))
        {
            _buttons[0].gameObject.SetActive(false);

            _buttons[1].gameObject.SetActive(true);
            _buttons[2].gameObject.SetActive(true);
        }

        if(Controls(2) && Controls(3))
        {
            _buttons[1].gameObject.SetActive(false);
            _buttons[2].gameObject.SetActive(false);

            _buttons[3].gameObject.SetActive(true);
            _buttons[4].gameObject.SetActive(true);
        }
        if(Controls(4) || Controls(5))
        {
            if (Controls(4)) _buttons[3].gameObject.SetActive(false);
            if (Controls(5)) _buttons[4].gameObject.SetActive(false);

            _buttons[5].gameObject.SetActive(true);
        }
    }

    private void CheckCampaignCompletion()
    {
        if (Controls(1) && !checkCmp1)
        {
            checkCmp1 = true;
            _campaignManager.CompleteCampaign1();
        }

        if (Controls(2) && Controls(3) && !checkCmp2)
        {
            checkCmp2 = true;
            _campaignManager.CompleteCampaign2();
        }

        if (Controls(4) && Controls(5) && Controls(6) && !checkCmp3)
        {
            checkCmp3 = true;
            _campaignManager.CompleteCampaign3();
        }
    }
}
