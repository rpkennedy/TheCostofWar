using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private int _totalPlatoons = 0;
    private int _totalReconPlanes = 0;
    private int _totalBombers = 0;
    private int _totalBattleships = 0;
    private int _totalCarriers = 0;

    private CampaignManager _campaignManager;
    public List<Unit> theFallen;

    [Header("Land")]
    public List<Platoon> platoons;

    [Header("Air")]
    public List<ReconPlane> reconPlanes;
    public List<Bomber> bombers;

    [Header("Sea")]
    public List<Battleship> battleships;
    public List<Carrier> carriers;

    [Header("Unit Prefabs")]
    public GameObject platoonPrefab;
    public GameObject recPlanePrefab;
    public GameObject bomberPrefab;
    public GameObject battleshipPrefab;
    public GameObject carrierPrefab;

    [Header("Operation Util")]
    public List<GameObject> vesselsEmbarked;
    public List<GameObject> unitsEmbarked;
    public List<GameObject> embarkedVitals;

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);

        if (DataStore.brain == null) DataStore.SetBrain(this);
        else
        {
            if (DataStore.brain != this) Destroy(this.gameObject);
        }

        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<CampaignManager>() != null)
            _campaignManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<CampaignManager>();

        platoons = new List<Platoon>();
        reconPlanes = new List<ReconPlane>();
        bombers = new List<Bomber>();
        battleships = new List<Battleship>();
        carriers = new List<Carrier>();

        vesselsEmbarked = new List<GameObject>();
        unitsEmbarked = new List<GameObject>();
        embarkedVitals = new List<GameObject>();
    }     
    
    public void EmbarkVessel()
    {
        UIManager ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        if (GameObject.Find(ui.vesselsAvailableDD.captionText.text) == null) return;
        GameObject go = GameObject.Find(ui.vesselsAvailableDD.captionText.text);

        vesselsEmbarked.Add(go);

        if (go.name.Contains("Carrier"))
        {
            foreach (Carrier carrier in carriers)
            {
                if (carrier == go.GetComponent<Carrier>())
                {
                    carriers.Remove(carrier);
                    return;
                }
            }
        }
        if (go.name.Contains("Battleship"))
        {
            foreach (Battleship ship in battleships)
            {
                if (ship == go.GetComponent<Battleship>())
                {
                    embarkedVitals.Add(go);
                    battleships.Remove(ship);
                    return;
                }
            }
        }        
    }

    public string GetEmbarkedVessel(int index)
    {
        return vesselsEmbarked[index].name;
    }

    public string GetEmbarkedUnit(int index)
    {
        return unitsEmbarked[index].name;
    }

    public void RemoveEmbarkedVessel()
    {
        UIManager ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        if (GameObject.Find(ui.vesselsSelectedDD.captionText.text) == null) return;
        GameObject go = GameObject.Find(ui.vesselsSelectedDD.captionText.text);
        if (go.name.Contains("Battleship")) battleships.Add(go.GetComponent<Battleship>());
        if (go.name.Contains("Carrier")) carriers.Add(go.GetComponent<Carrier>());
        vesselsEmbarked.Remove(go);
    }

    public void EmbarkUnit()
    {
        UIManager ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        if (GameObject.Find(ui.unitsAvailableDD.captionText.text) == null) return;
        GameObject go = GameObject.Find(ui.unitsAvailableDD.captionText.text);

        if (go.GetComponent<Unit>().type == UnitType.Land)
        {
            Debug.Log("Embarking land unit - slots left: " + ui.landSlots);
            if (ui.landSlots == 0) return;
        }
        if (go.GetComponent<Unit>().type == UnitType.Air)
        {
            if (ui.airSlots == 0) return;
        }

        unitsEmbarked.Add(go);
        Debug.Log("Embarked land unit - list size: " + unitsEmbarked.Count);
        if (go.name.Contains("Platoon"))
        {
            foreach (Platoon plat in platoons)
            {
                if (plat == go.GetComponent<Platoon>())
                {
                    embarkedVitals.Add(go);
                    platoons.Remove(plat); 
                    foreach (GameObject carrier in vesselsEmbarked)
                    {
                        if (carrier.name.Contains("Carrier"))
                        {
                            if (carrier.GetComponent<Carrier>().platoonsStored.Count != carrier.GetComponent<Carrier>().landUnitMaxCapacity)
                            {
                                carrier.GetComponent<Carrier>().StorePlatoon(plat);
                            }
                        }
                    }
                    return;
                }
            }
        }
        if (go.name.Contains("Recon")) 
        {
            foreach (ReconPlane recon in reconPlanes)
            {
                if (recon == go.GetComponent<ReconPlane>())
                {
                    reconPlanes.Remove(recon);
                    foreach (GameObject carrier in vesselsEmbarked)
                    {
                        if (carrier.name.Contains("Carrier"))
                        {
                            if (carrier.GetComponent<Carrier>().reconPlanesStored.Count + carrier.GetComponent<Carrier>().bombersStored.Count < carrier.GetComponent<Carrier>().airUnitMaxCapacity)
                            {
                                carrier.GetComponent<Carrier>().StoreRecon(recon);
                            }
                        }
                    }
                    return;
                }
            }
        }
        if (go.name.Contains("Bomber"))
        {
            foreach (Bomber bomber in bombers)
            {
                if (bomber == go.GetComponent<Bomber>())
                {
                    bombers.Remove(bomber);
                    foreach (GameObject carrier in vesselsEmbarked)
                    {
                        if (carrier.name.Contains("Carrier"))
                        {
                            if (carrier.GetComponent<Carrier>().reconPlanesStored.Count + carrier.GetComponent<Carrier>().bombersStored.Count < carrier.GetComponent<Carrier>().airUnitMaxCapacity)
                            {
                                carrier.GetComponent<Carrier>().StoreBomber(bomber);
                            }
                        }
                    }
                    return;
                }
            }
        }
    }

    public int GetLandUnits()
    {
        int numLandUnits = 0;
        foreach(GameObject unit in unitsEmbarked)
        {
            if(unit.GetComponent<Unit>().type == UnitType.Land)
            {
                numLandUnits++;
            }
        }
        return numLandUnits;
    }
    public int GetAirUnits()
    {
        int numAirUnits = 0;
        foreach (GameObject unit in unitsEmbarked)
        {
            if (unit.GetComponent<Unit>().type == UnitType.Air)
            {
                numAirUnits++;
            }
        }
        return numAirUnits;
    }

    public void RemoveEmbarkedUnit()
    {
        UIManager ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        if (GameObject.Find(ui.unitsSelectedDD.captionText.text) == null) return;
        GameObject go = GameObject.Find(ui.unitsSelectedDD.captionText.text);
        if (go.name.Contains("Platoon")) platoons.Add(go.GetComponent<Platoon>());
        if (go.name.Contains("Recon")) reconPlanes.Add(go.GetComponent<ReconPlane>());
        if (go.name.Contains("Bomber")) bombers.Add(go.GetComponent<Bomber>());
        unitsEmbarked.Remove(go);
    }

    public void AddPlatoon()
    {
        GameObject platoon = Instantiate(platoonPrefab);

        if (!_campaignManager.Deduct(platoon.GetComponent<Platoon>().cost))
        {
            Destroy(platoon);
            return;
        }

        platoons.Add(platoon.GetComponent<Platoon>());
        _totalPlatoons++;

        if (_totalPlatoons % 10 == 1)
        {
            platoon.GetComponent<Platoon>().unitName = _totalPlatoons + "st Platoon";
        }
        if (_totalPlatoons % 10 == 2)
        {
            platoon.GetComponent<Platoon>().unitName = _totalPlatoons + "nd Platoon";
        }
        if (_totalPlatoons % 10 == 3)
        {
            platoon.GetComponent<Platoon>().unitName = _totalPlatoons + "rd Platoon";
        }
        if (_totalPlatoons % 10 >= 4 || platoons.Count % 10 == 0)
        {
            platoon.GetComponent<Platoon>().unitName = _totalPlatoons + "th Platoon";
        }
        platoon.name = platoon.GetComponent<Platoon>().unitName;
    }

    public void AddReconPlane()
    {
        GameObject reconPlane = Instantiate(recPlanePrefab);

        if (!_campaignManager.Deduct(reconPlane.GetComponent<ReconPlane>().cost))
        {
            Destroy(reconPlane);
            return;
        }

        reconPlanes.Add(reconPlane.GetComponent<ReconPlane>());
        _totalReconPlanes++;

        if (_totalReconPlanes % 10 == 1)
        {
            reconPlane.GetComponent<ReconPlane>().unitName = _totalReconPlanes + "st Recon Plane";
        }
        if (_totalReconPlanes % 10 == 2)
        {
            reconPlane.GetComponent<ReconPlane>().unitName = _totalReconPlanes + "nd Recon Plane";
        }
        if (_totalReconPlanes % 10 == 3)
        {
            reconPlane.GetComponent<ReconPlane>().unitName = _totalReconPlanes + "rd Recon Plane";
        }
        if (_totalReconPlanes % 10 >= 4 || _totalReconPlanes % 10 == 0)
        {
            reconPlane.GetComponent<ReconPlane>().unitName = _totalReconPlanes + "th Recon Plane";
        }
        reconPlane.name = reconPlane.GetComponent<ReconPlane>().unitName;
    }

    public void AddBomber()
    {
        GameObject bomber = Instantiate(bomberPrefab);

        if (!_campaignManager.Deduct(bomber.GetComponent<Bomber>().cost))
        {
            Destroy(bomber);
            return;
        }

        bombers.Add(bomber.GetComponent<Bomber>());
        _totalBombers++;

        if (_totalBombers % 10 == 1)
        {
            bomber.GetComponent<Bomber>().unitName = _totalBombers + "st Bomber";
        }
        if (_totalBombers % 10 == 2)
        {
            bomber.GetComponent<Bomber>().unitName = _totalBombers + "nd Bomber";
        }
        if (_totalBombers % 10 == 3)
        {
            bomber.GetComponent<Bomber>().unitName = _totalBombers + "rd Bomber";
        }
        if (_totalBombers % 10 >= 4 || _totalBombers % 10 == 0)
        {
            bomber.GetComponent<Bomber>().unitName = _totalBombers + "th Bomber";
        }
        bomber.name = bomber.GetComponent<Bomber>().unitName;
    }

    public void AddBattleship()
    {
        GameObject battleship = Instantiate(battleshipPrefab);

        if (!_campaignManager.Deduct(battleship.GetComponent<Battleship>().cost))
        {
            Destroy(battleship);
            return;
        }

        battleships.Add(battleship.GetComponent<Battleship>());
        _totalBattleships++;

        if (_totalBattleships % 10 == 1)
        {
            battleship.GetComponent<Battleship>().unitName = _totalBattleships + "st Battleship";
        }
        if (_totalBattleships % 10 == 2)
        {
            battleship.GetComponent<Battleship>().unitName = _totalBattleships + "nd Battleship";
        }
        if (_totalBattleships % 10 == 3)
        {
            battleship.GetComponent<Battleship>().unitName = _totalBattleships + "rd Battleship";
        }
        if (_totalBattleships % 10 >= 4 || _totalBattleships % 10 == 0)
        {
            battleship.GetComponent<Battleship>().unitName = _totalBattleships + "th Battleship";
        }
        battleship.name = battleship.GetComponent<Battleship>().unitName;
    }

    public void AddCarrier()
    {
        GameObject carrier = Instantiate(carrierPrefab);

        if (!_campaignManager.Deduct(carrier.GetComponent<Carrier>().cost))
        {
            Destroy(carrier);
            return;
        }

        carriers.Add(carrier.GetComponent<Carrier>());
        _totalCarriers++;

        if (_totalCarriers % 10 == 1)
        {
            carrier.GetComponent<Carrier>().unitName = _totalCarriers + "st Carrier";
        }
        if (_totalCarriers % 10 == 2)
        {
            carrier.GetComponent<Carrier>().unitName = _totalCarriers + "nd Carrier";
        }
        if (_totalCarriers % 10 == 3)
        {
            carrier.GetComponent<Carrier>().unitName = _totalCarriers + "rd Carrier";
        }
        if (_totalCarriers % 10 >= 4 || _totalCarriers % 10 == 0)
        {
            carrier.GetComponent<Carrier>().unitName = _totalCarriers + "th Carrier";
        }
        carrier.name = carrier.GetComponent<Carrier>().unitName;
    }

    public void Victory()
    {
        //remove all after the end
        DataStore.islandControl[GameObject.FindGameObjectWithTag("Operation").GetComponent<Operation>().islandID] = true;
        foreach (GameObject go in unitsEmbarked)
        {
            if (go.name.Contains("Platoon"))
            {
                platoons.Add(go.GetComponent<Platoon>());
                go.GetComponent<Platoon>().HideSprites();
            }
            if (go.name.Contains("Recon"))
            {
                reconPlanes.Add(go.GetComponent<ReconPlane>());
                go.GetComponent<SpriteRenderer>().enabled = false;
            }
            if (go.name.Contains("Bomber"))
            {
                bombers.Add(go.GetComponent<Bomber>());
                go.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        unitsEmbarked.Clear();
        foreach (GameObject go in vesselsEmbarked)
        {
            if (go.name.Contains("Carrier"))
            {
                carriers.Add(go.GetComponent<Carrier>());
                go.GetComponent<SpriteRenderer>().enabled = false;
            }
            if (go.name.Contains("Battleship"))
            {
                battleships.Add(go.GetComponent<Battleship>());
                go.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        vesselsEmbarked.Clear();
        this.GetComponent<SceneHandler>().LoadCampaign();     //scene
    }

    public void WithdrawalForces()
    {
        foreach(GameObject go in unitsEmbarked)
        {
            if (go.name.Contains("Platoon"))
            {
                platoons.Add(go.GetComponent<Platoon>());
                go.GetComponent<Platoon>().HideSprites();
            }
            if (go.name.Contains("Recon"))
            {
                reconPlanes.Add(go.GetComponent<ReconPlane>());
                go.GetComponent<SpriteRenderer>().enabled = false;
            }
            if (go.name.Contains("Bomber"))
            {
                bombers.Add(go.GetComponent<Bomber>());
                go.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        unitsEmbarked.Clear();
        foreach(GameObject go in vesselsEmbarked)
        {
            if (go.name.Contains("Carrier"))
            {
                carriers.Add(go.GetComponent<Carrier>());
                go.GetComponent<SpriteRenderer>().enabled = false;
            }
            if (go.name.Contains("Battleship"))
            {
                battleships.Add(go.GetComponent<Battleship>());
                go.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        vesselsEmbarked.Clear();
        this.GetComponent<SceneHandler>().LoadCampaign();     //scene
    }

    public void Defeat()
    {
        WithdrawalForces();
    }
}
