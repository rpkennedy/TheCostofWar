using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public CampaignManager _campaignManager;
    public UnitManager units;

    [Header("UPGRADE UI")]
    public Text totalPlatoons;
    public Text platoonsUpgraded1;
    public Text platoonsUpgraded2;

    public Text totalReconPlanes;
    public Text reconUpgraded1;
    public Text reconUpgraded2;

    public Text totalBombers;
    public Text bombersUpgraded1;
    public Text bombersUpgraded2;

    public Text totalBattleships;
    public Text battleshipsUpgraded1;
    public Text battleshipsUpgraded2;

    public Text totalCarriers;
    public Text carriersUpgraded1;
    public Text carriersUpgraded2;

    [Header("Upgrade Buy Buttons")]
    public Button buttPlatoon1;
    public Button buttPlatoon2;

    public Button buttRecon1;
    public Button buttRecon2;

    public Button buttBomber1;
    public Button buttBomber2;

    public Button buttShip1;
    public Button buttShip2;

    public Button buttCarrier1;
    public Button buttCarrier2;

    [Header("HEAL UI")]
    public Dropdown platoonDrop;
    public Dropdown reconDrop;
    public Dropdown bomberDrop;
    public Dropdown battleshipDrop;
    public Dropdown carrierDrop;

    public List<Text> platoonHealTexts;
    public List<Text> reconHealTexts;
    public List<Text> bomberHealTexts;
    public List<Text> battleshipHealTexts;
    public List<Text> carrierHealTexts;

    public List<Text> platoonHealCostTexts;
    public List<Text> reconHealCostTexts;
    public List<Text> bomberHealCostTexts;
    public List<Text> battleshipHealCostTexts;
    public List<Text> carrierHealCostTexts;

    public List<Button> platoonHealButtons;
    public List<Button> reconHealButtons;
    public List<Button> bomberHealButtons;
    public List<Button> battleshipHealButtons;
    public List<Button> carrierHealButtons;

    [Header("Unit Selection UI")]
    public Dropdown vesselsAvailableDD;
    public Dropdown vesselsSelectedDD;

    public Dropdown unitsAvailableDD;
    public Dropdown unitsSelectedDD;

    public Text freeLandSlots;
    public Text freeAirSlots;

    public int landSlots;
    public int airSlots;

    [Header("Heal Costs")]
    public int platoonHealCost;
    public int reconHealCost;
    public int bomberHealCost;
    public int battleshipHealCost;
    public int carrierHealCost;

    public void ConfigureVesselsUI()
    {
        vesselsAvailableDD.ClearOptions();
        vesselsAvailableDD.captionText.text = "Choose Unit";

        List<string> options = new List<string>();

        foreach (Battleship ship in units.battleships)
        {
            options.Add(ship.gameObject.name);
        }
        foreach (Carrier carrier in units.carriers)
        {
            options.Add(carrier.gameObject.name);
        }
        vesselsAvailableDD.AddOptions(options);

        vesselsSelectedDD.ClearOptions();
        vesselsSelectedDD.captionText.text = "Choose Unit";

        List<string> options2 = new List<string>();

        for (int i = 0; i < units.vesselsEmbarked.Count; i++)
        {
            options2.Add(units.GetEmbarkedVessel(i));
        }
        vesselsSelectedDD.AddOptions(options2);
    }

    public void ConfigureUnitsUI()
    {
        landSlots = 0;
        airSlots = 0;
        foreach(GameObject unit in units.vesselsEmbarked)
        {
            if (unit.name.Contains("Carrier"))
            {
                landSlots += unit.GetComponent<Carrier>().landUnitMaxCapacity;
                airSlots += unit.GetComponent<Carrier>().airUnitMaxCapacity;
            }
        }

        landSlots -= units.GetLandUnits();
        airSlots -= units.GetAirUnits();

        freeLandSlots.text = landSlots.ToString();
        freeAirSlots.text = airSlots.ToString();

        unitsAvailableDD.ClearOptions();
        unitsAvailableDD.captionText.text = "Choose Unit";

        List<string> options = new List<string>();

        foreach (Platoon platoon in units.platoons)
        {
            options.Add(platoon.gameObject.name);
        }
        foreach (ReconPlane recon in units.reconPlanes)
        {
            options.Add(recon.gameObject.name);
        }
        foreach (Bomber bomber in units.bombers)
        {
            options.Add(bomber.gameObject.name);
        }
        unitsAvailableDD.AddOptions(options);



        unitsSelectedDD.ClearOptions();
        unitsSelectedDD.captionText.text = "Choose Unit";

        List<string> options2 = new List<string>();

        for (int i = 0; i < units.unitsEmbarked.Count; i++)
        {
            options2.Add(units.GetEmbarkedUnit(i));
        }
        unitsSelectedDD.AddOptions(options2);    
    }

    public string GetVesselAvailableString()
    {
        return vesselsAvailableDD.itemText.text;
    }
    public string GetVesselSelectedString()
    {
        return vesselsSelectedDD.itemText.text;
    }

    public string GetUnitAvailableString()
    {
        return unitsAvailableDD.itemText.text;
    }
    public string GetUnitSelectedString()
    {
        return unitsAvailableDD.itemText.text;
    }

    public void ConfigureUpdateUI()
    {
        totalPlatoons.text = "Total Units: " + units.platoons.Count;
        platoonsUpgraded1.text = "Units w/ Upgrade: " + platoonUpgrade1();
        if (platoonUpgrade1() == units.platoons.Count) buttPlatoon1.interactable = false;
        else
        {
            buttPlatoon1.interactable = true;
            buttPlatoon1.enabled = true;
        }
        platoonsUpgraded2.text = "Units w/ Upgrade: " + platoonUpgrade2();
        if (platoonUpgrade2() == units.platoons.Count) buttPlatoon2.interactable = false;
        else
        {
            buttPlatoon2.interactable = true;
            buttPlatoon2.enabled = true;
        }

        totalReconPlanes.text = "Total Units: " + units.reconPlanes.Count;
        reconUpgraded1.text = "Units w/ Upgrade: " + reconUpgrade1();
        if (reconUpgrade1() == units.reconPlanes.Count) buttRecon1.interactable = false;
        else
        {
            buttRecon1.interactable = true;
            buttRecon1.enabled = true;
        }
        reconUpgraded2.text = "Units w/ Upgrade: " + reconUpgrade2();
        if (reconUpgrade2() == units.reconPlanes.Count) buttRecon2.interactable = false;
        else
        {
            buttRecon2.interactable = true;
            buttRecon2.enabled = true;
        }

        totalBombers.text = "Total Units: " + units.bombers.Count;
        bombersUpgraded1.text = "Units w/ Upgrade: " + bomberUpgrade1();
        if (bomberUpgrade1() == units.bombers.Count) buttBomber1.interactable = false;
        else
        {
            buttBomber1.interactable = true;
            buttBomber1.enabled = true;
        }
        bombersUpgraded2.text = "Units w/ Upgrade: " + bomberUpgrade2();
        if (bomberUpgrade2() == units.bombers.Count) buttBomber2.interactable = false;
        else
        {
            buttBomber2.interactable = true;
            buttBomber2.enabled = true;
        }

        totalCarriers.text = "Total Units: " + units.carriers.Count;
        carriersUpgraded1.text = "Units w/ Upgrade: " + carrierUpgrade1();
        if (carrierUpgrade1() == units.carriers.Count) buttCarrier1.interactable = false;
        else
        {
            buttCarrier1.interactable = true;
            buttCarrier1.enabled = true;
        }
        carriersUpgraded2.text = "Units w/ Upgrade: " + carrierUpgrade2();
        if (carrierUpgrade2() == units.carriers.Count) buttCarrier2.interactable = false;
        else
        {
            buttCarrier2.interactable = true;
            buttCarrier2.enabled = true;
        }

        totalBattleships.text = "Total Units: " + units.battleships.Count;
        battleshipsUpgraded1.text = "Units w/ Upgrade: " + battleshipUpgrade1();
        if (battleshipUpgrade1() == units.battleships.Count) buttShip1.interactable = false;
        else
        {
            buttShip1.interactable = true;
            buttShip1.enabled = true;
        }
        battleshipsUpgraded2.text = "Units w/ Upgrade: " + battleshipUpgrade2();
        if (battleshipUpgrade2() == units.battleships.Count) buttShip2.interactable = false;
        else
        {
            buttShip2.interactable = true;
            buttShip2.enabled = true;
        }
    }

    public void ConfigureHealUI()
    {
        SetupHealthDropdowns();
        SetupHealthText();

        for(int i = 1; i <= 5; i++)
        {
            SetupHealthButtons(i);
        }
    }

    public void SetupHealthButtons(int id)
    {
        GameObject unit;
        if(id == 1)
        {
            platoonHealButtons[0].interactable = false;
            platoonHealCostTexts[0].text = "$ " + platoonHealCost;

            platoonHealButtons[1].interactable = false;
            platoonHealCostTexts[1].text = "$ " + platoonHealCost;

            if (GameObject.Find(platoonDrop.captionText.text) == null) return;
            unit = GameObject.Find(platoonDrop.captionText.text);

            Platoon platoon = unit.GetComponent<Platoon>();
            if(platoon.GetHealth() < platoon.maxHealth)
            {
                platoonHealButtons[0].interactable = true;
                platoonHealCostTexts[0].text = "$ " + platoonHealCost * (platoon.maxHealth-platoon.GetHealth());

                platoonHealButtons[1].interactable = true;
                platoonHealCostTexts[1].text = "$ " + platoonHealCost;
            }
        }
        if (id == 2)
        {
            reconHealButtons[0].interactable = false;
            reconHealCostTexts[0].text = "$ " + reconHealCost;

            reconHealButtons[1].interactable = false;
            reconHealCostTexts[1].text = "$ " + reconHealCost;

            if (GameObject.Find(reconDrop.captionText.text) == null) return;
            unit = GameObject.Find(reconDrop.captionText.text);

            ReconPlane recon = unit.GetComponent<ReconPlane>();
            if (recon.GetHealth() < recon.maxHealth)
            {
                reconHealButtons[0].interactable = true;
                reconHealCostTexts[0].text = "$ " + reconHealCost * (recon.maxHealth - recon.GetHealth());

                reconHealButtons[1].interactable = true;
                reconHealCostTexts[1].text = "$ " + reconHealCost;
            }
        }
        if (id == 3)
        {
            bomberHealButtons[0].interactable = false;
            bomberHealCostTexts[0].text = "$ " + bomberHealCost;

            bomberHealButtons[1].interactable = false;
            bomberHealCostTexts[1].text = "$ " + bomberHealCost;

            if (GameObject.Find(bomberDrop.captionText.text) == null) return;
            unit = GameObject.Find(bomberDrop.captionText.text);

            Bomber bomber = unit.GetComponent<Bomber>();
            if (bomber.GetHealth() < bomber.maxHealth)
            {
                bomberHealButtons[0].interactable = true;
                bomberHealCostTexts[0].text = "$ " + bomberHealCost * (bomber.maxHealth - bomber.GetHealth());

                bomberHealButtons[1].interactable = true;
                bomberHealCostTexts[1].text = "$ " + bomberHealCost;
            }
        }
        if (id == 4)
        {
            battleshipHealButtons[0].interactable = false;
            battleshipHealCostTexts[0].text = "$ " + battleshipHealCost;

            battleshipHealButtons[1].interactable = false;
            battleshipHealCostTexts[1].text = "$ " + battleshipHealCost;

            if (GameObject.Find(battleshipDrop.captionText.text) == null) return;
            unit = GameObject.Find(battleshipDrop.captionText.text);

            Battleship battleship = unit.GetComponent<Battleship>();
            if (battleship.GetHealth() < battleship.maxHealth)
            {
                battleshipHealButtons[0].interactable = true;
                battleshipHealCostTexts[0].text = "$ " + battleshipHealCost * (battleship.maxHealth - battleship.GetHealth());

                battleshipHealButtons[1].interactable = true;
                battleshipHealCostTexts[1].text = "$ " + battleshipHealCost;
            }
        }
        if (id == 5)
        {
            carrierHealButtons[0].interactable = false;
            carrierHealCostTexts[0].text = "$ " + carrierHealCost;

            carrierHealButtons[1].interactable = false;
            carrierHealCostTexts[1].text = "$ " + carrierHealCost;

            if (GameObject.Find(carrierDrop.captionText.text) == null) return;
            unit = GameObject.Find(carrierDrop.captionText.text);

            Carrier carrier = unit.GetComponent<Carrier>();
            if (carrier.GetHealth() < carrier.maxHealth)
            {
                carrierHealButtons[0].interactable = true;
                carrierHealCostTexts[0].text = "$ " + carrierHealCost * (carrier.maxHealth - carrier.GetHealth());

                carrierHealButtons[1].interactable = true;
                carrierHealCostTexts[1].text = "$ " + carrierHealCost;
            }
        }
    }

    private void SetupHealthText()
    {
        platoonHealTexts[0].text = "Total Units: " + units.platoons.Count;
        platoonHealTexts[1].text = "Undamaged Units: " + GetUndamagedInt(1);
        platoonHealTexts[2].text = "Damaged Units: " + (units.platoons.Count - GetUndamagedInt(1));

        reconHealTexts[0].text = "Total Units: " + units.reconPlanes.Count;
        reconHealTexts[1].text = "Undamaged Units: " + GetUndamagedInt(2);
        reconHealTexts[2].text = "Damaged Units: " + (units.reconPlanes.Count - GetUndamagedInt(2));

        bomberHealTexts[0].text = "Total Units: " + units.bombers.Count;
        bomberHealTexts[1].text = "Undamaged Units: " + GetUndamagedInt(3);
        bomberHealTexts[2].text = "Damaged Units: " + (units.bombers.Count - GetUndamagedInt(3));

        battleshipHealTexts[0].text = "Total Units: " + units.battleships.Count;
        battleshipHealTexts[1].text = "Undamaged Units: " + GetUndamagedInt(4);
        battleshipHealTexts[2].text = "Damaged Units: " + (units.battleships.Count - GetUndamagedInt(4));

        carrierHealTexts[0].text = "Total Units: " + units.carriers.Count;
        carrierHealTexts[1].text = "Undamaged Units: " + GetUndamagedInt(5);
        carrierHealTexts[2].text = "Damaged Units: " + (units.carriers.Count - GetUndamagedInt(5));
    }

    private int GetUndamagedInt(int id)
    {
        int undamaged = 0;

        if(id == 1)
        {
            foreach(Platoon platoon in units.platoons)
            {
                if (platoon.GetHealth() == platoon.maxHealth) undamaged++;
            }
        }
        if (id == 2)
        {
            foreach (ReconPlane recon in units.reconPlanes)
            {
                if (recon.GetHealth() == recon.maxHealth) undamaged++;
            }
        }
        if (id == 3)
        {
            foreach (Bomber bomber in units.bombers)
            {
                if (bomber.GetHealth() == bomber.maxHealth) undamaged++;
            }
        }
        if (id == 4)
        {
            foreach (Battleship battleship in units.battleships)
            {
                if (battleship.GetHealth() == battleship.maxHealth) undamaged++;
            }
        }
        if (id == 5)
        {
            foreach (Carrier carrier in units.carriers)
            {
                if (carrier.GetHealth() == carrier.maxHealth) undamaged++;
            }
        }

        return undamaged;
    }

    private void SetupHealthDropdowns()
    {
        //platoon
        if (units.platoons.Count == 0)
        {
            platoonDrop.captionText.text = "Not Available";
        }
        else
        {
            platoonDrop.ClearOptions();
            platoonDrop.captionText.text = "Choose Unit";

            List<string> options = new List<string>();

            foreach (Platoon plat in units.platoons)
            {
                options.Add(plat.gameObject.name);
            }
            platoonDrop.AddOptions(options);
        }

        //recon plane
        if (units.reconPlanes.Count == 0)
        {
            reconDrop.captionText.text = "Not Available";
        }
        else
        {
            reconDrop.ClearOptions();
            reconDrop.captionText.text = "Choose Unit";

            List<string> options = new List<string>();

            foreach (ReconPlane rec in units.reconPlanes)
            {
                options.Add(rec.gameObject.name);
            }
            reconDrop.AddOptions(options);
        }

        //bomber
        if (units.bombers.Count == 0)
        {
            bomberDrop.captionText.text = "Not Available";
        }
        else
        {
            bomberDrop.ClearOptions();
            bomberDrop.captionText.text = "Choose Unit";

            List<string> options = new List<string>();

            foreach (Bomber bomb in units.bombers)
            {
                options.Add(bomb.gameObject.name);
            }
            bomberDrop.AddOptions(options);
        }

        //battleship
        if (units.battleships.Count == 0)
        {
            battleshipDrop.captionText.text = "Not Available";
        }
        else
        {
            battleshipDrop.ClearOptions();
            battleshipDrop.captionText.text = "Choose Unit";

            List<string> options = new List<string>();

            foreach (Battleship ship in units.battleships)
            {
                options.Add(ship.gameObject.name);
            }
            battleshipDrop.AddOptions(options);
        }

        //carrier
        if (units.carriers.Count == 0)
        {
            carrierDrop.captionText.text = "Not Available";
        }
        else
        {
            carrierDrop.ClearOptions();
            carrierDrop.captionText.text = "Choose Unit";

            List<string> options = new List<string>();

            foreach (Carrier carr in units.carriers)
            {
                options.Add(carr.gameObject.name);
            }
            carrierDrop.AddOptions(options);
        }
    }


    //Platoon Upgrades
    public int platoonUpgrade1()
    {
        int numUpgraded = 0;
        foreach(Platoon plat in units.platoons)
        {
            if (plat.hasExtraMove) numUpgraded++;
        }
        return numUpgraded;
    }
    public void BuyPlatoonUpg1()
    {
        foreach(Platoon plat in units.platoons)
        {
            if (!plat.hasExtraMove)
            {
                plat.hasExtraMove = _campaignManager.Deduct(30000);
                ConfigureUpdateUI();
                return;
            }
        }
    }
    public int platoonUpgrade2()
    {
        int numUpgraded = 0;
        foreach (Platoon plat in units.platoons)
        {
            if (plat.hasExtraDamage) numUpgraded++;
        }
        return numUpgraded;
    }
    public void BuyPlatoonUpg2()
    {
        foreach (Platoon plat in units.platoons)
        {
            if (!plat.hasExtraDamage)
            {
                plat.hasExtraDamage = _campaignManager.Deduct(60000);
                ConfigureUpdateUI();
                return;
            }
        }
    }

    //Recon Plane Upgrades
    public int reconUpgrade1()
    {
        int numUpgraded = 0;
        foreach (ReconPlane recon in units.reconPlanes)
        {
            if (recon.hasLongerScan) numUpgraded++;
        }
        return numUpgraded;
    }
    public void BuyReconUpg1()
    {
        foreach (ReconPlane rec in units.reconPlanes)
        {
            if (!rec.hasLongerScan)
            {
                rec.hasLongerScan = _campaignManager.Deduct(50000);
                ConfigureUpdateUI();
                return;
            }
        }
    }
    public int reconUpgrade2()
    {
        int numUpgraded = 0;
        foreach (ReconPlane recon in units.reconPlanes)
        {
            if (recon.hasExtraHealth) numUpgraded++;
        }
        return numUpgraded;
    }
    public void BuyReconUpg2()
    {
        foreach (ReconPlane rec in units.reconPlanes)
        {
            if (!rec.hasExtraHealth)
            {
                rec.hasExtraHealth = _campaignManager.Deduct(80000);
                ConfigureUpdateUI();
                return;
            }
        }
    }

    //Bomber Upgrades
    public int bomberUpgrade1()
    {
        int numUpgraded = 0;
        foreach (Bomber bomber in units.bombers)
        {
            if (bomber.hasExplosiveOrdinance) numUpgraded++;
        }
        return numUpgraded;
    }
    public void BuyBomberUpg1()
    {
        foreach (Bomber bomb in units.bombers)
        {
            if (!bomb.hasExplosiveOrdinance)
            {
                bomb.hasExplosiveOrdinance = _campaignManager.Deduct(70000);
                ConfigureUpdateUI();
                return;
            }
        }
    }
    public int bomberUpgrade2()
    {
        int numUpgraded = 0;
        foreach (Bomber bomber in units.bombers)
        {
            if (bomber.hasExtraOrdinance) numUpgraded++;
        }
        return numUpgraded;
    }
    public void BuyBomberUpg2()
    {
        foreach (Bomber bomb in units.bombers)
        {
            if (!bomb.hasExtraOrdinance)
            {
                bomb.hasExtraOrdinance = _campaignManager.Deduct(50000);
                ConfigureUpdateUI();
                return;
            }
        }
    }

    //Carrier Upgrades
    public int carrierUpgrade1()
    {
        int numUpgraded = 0;
        foreach (Carrier carrier in units.carriers)
        {
            if (carrier.landUnitCapacity == 4) numUpgraded++;
        }
        return numUpgraded;
    }
    public void BuyCarrierUpg1()
    {
        foreach (Carrier carrier in units.carriers)
        {
            if (carrier.landUnitCapacity != 4)
            {
                if (!_campaignManager.Deduct(75000)) return;
                carrier.landUnitCapacity = 4;
                ConfigureUpdateUI();
                return;
            }
        }
    }
    public int carrierUpgrade2()
    {
        int numUpgraded = 0;
        foreach (Carrier carrier in units.carriers)
        {
            if (carrier.airUnitCapacity == 2) numUpgraded++;
        }
        return numUpgraded;
    }
    public void BuyCarrierUpg2()
    {
        foreach (Carrier carrier in units.carriers)
        {
            if (carrier.airUnitCapacity != 2)
            {
                if (!_campaignManager.Deduct(100000)) return;
                carrier.UpgradeAirCapacity();
                ConfigureUpdateUI();
                return;
            }
        }
    }

    //Battleship Upgrades
    public int battleshipUpgrade1()
    {
        int numUpgraded = 0;
        foreach (Battleship ship in units.battleships)
        {
            if (ship.hasExtraMove) numUpgraded++;
        }
        return numUpgraded;
    }
    public void BuyBattleshipUpg1()
    {
        foreach (Battleship ship in units.battleships)
        {
            if (!ship.hasExtraMove)
            {
                ship.hasExtraMove = _campaignManager.Deduct(20000);
                ConfigureUpdateUI();
                return;
            }
        }
    }
    public int battleshipUpgrade2()
    {
        int numUpgraded = 0;
        foreach (Battleship ship in units.battleships)
        {
            if (ship.hasExtraHealth) numUpgraded++;
        }
        return numUpgraded;
    }
    public void BuyBattleshipUpg2()
    {
        foreach (Battleship ship in units.battleships)
        {
            if (!ship.hasExtraHealth)
            {
                ship.hasExtraHealth = _campaignManager.Deduct(20000);
                ConfigureUpdateUI();
                return;
            }
        }
    }

    //platoon heal
    public void platoonHeal(bool fullHeal)
    {
        if (GameObject.Find(platoonDrop.captionText.text) == null) return;
        GameObject unit = GameObject.Find(platoonDrop.captionText.text);
        if (fullHeal)
        {
            int cost = (unit.GetComponent<Platoon>().maxHealth - unit.GetComponent<Platoon>().GetHealth()) * platoonHealCost;
            if (_campaignManager.Deduct(cost))
            {
                unit.GetComponent<Platoon>().SetHealth();
            }           
        }
        else
        {
            if (_campaignManager.Deduct(platoonHealCost))
            {
                unit.GetComponent<Platoon>().Heal();
            }
        }
    }

    //recon heal
    public void reconHeal(bool fullHeal)
    {
        if (GameObject.Find(reconDrop.captionText.text) == null) return;
        GameObject unit = GameObject.Find(reconDrop.captionText.text);
        if (fullHeal)
        {
            int cost = (unit.GetComponent<ReconPlane>().maxHealth - unit.GetComponent<ReconPlane>().GetHealth()) * reconHealCost;
            if (_campaignManager.Deduct(cost))
            {
                unit.GetComponent<ReconPlane>().SetHealth();
            }
        }
        else
        {
            if (_campaignManager.Deduct(reconHealCost))
            {
                unit.GetComponent<ReconPlane>().Heal();
            }
        }
    }

    //bomber heal
    public void bomberHeal(bool fullHeal)
    {
        if (GameObject.Find(bomberDrop.captionText.text) == null) return;
        GameObject unit = GameObject.Find(bomberDrop.captionText.text);
        if (fullHeal)
        {
            int cost = (unit.GetComponent<Bomber>().maxHealth - unit.GetComponent<Bomber>().GetHealth()) * bomberHealCost;
            if (_campaignManager.Deduct(cost))
            {
                unit.GetComponent<Bomber>().SetHealth();
            }
        }
        else
        {
            if (_campaignManager.Deduct(bomberHealCost))
            {
                unit.GetComponent<Bomber>().Heal();
            }
        }
    }

    //carrier heal
    public void carrierHeal(bool fullHeal)
    {
        if (GameObject.Find(carrierDrop.captionText.text) == null) return;
        GameObject unit = GameObject.Find(carrierDrop.captionText.text);
        if (fullHeal)
        {
            int cost = (unit.GetComponent<Carrier>().maxHealth - unit.GetComponent<Carrier>().GetHealth()) * carrierHealCost;
            if (_campaignManager.Deduct(cost))
            {
                unit.GetComponent<Carrier>().SetHealth();
            }
        }
        else
        {
            if (_campaignManager.Deduct(carrierHealCost))
            {
                unit.GetComponent<Carrier>().Heal();
            }
        }
    }

    //battleship heal
    public void battleshipHeal(bool fullHeal)
    {
        if (GameObject.Find(battleshipDrop.captionText.text) == null) return;
        GameObject unit = GameObject.Find(battleshipDrop.captionText.text);
        if (fullHeal)
        {
            int cost = (unit.GetComponent<Battleship>().maxHealth - unit.GetComponent<Battleship>().GetHealth()) * battleshipHealCost;
            if (_campaignManager.Deduct(cost))
            {
                unit.GetComponent<Battleship>().SetHealth();
            }
        }
        else
        {
            if (_campaignManager.Deduct(battleshipHealCost))
            {
                unit.GetComponent<Battleship>().Heal();
            }
        }
    }
}
