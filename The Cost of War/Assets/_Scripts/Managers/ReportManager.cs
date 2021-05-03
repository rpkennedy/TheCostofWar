using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ReportManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _report;
    private Island _island;
    private Color _dotColor;

    [Header("Threats")]
    public Text aerialThreatTxt;
    public Text landThreatTxt;
    public Text navalThreatTxt;

    [Header("Threat Level TXT Prefabs")]
    public List<Text> threatLevelTxts;

    [Header("Difficulty Dots")]
    public List<Image> dots;

    private void Awake()
    {
        _dotColor = dots[0].color;
    }

    public void ConfigureReport(Island island)
    {
        DataStore.AttackingIslandID = island.id;
        _island = island;
        _report.transform.position = new Vector3(island.reportPosition.x, island.reportPosition.y, -1.8f );

        aerialThreatTxt.text = threatLevelTxts[island.aerialThreat].text;
        aerialThreatTxt.color = threatLevelTxts[island.aerialThreat].color;

        landThreatTxt.text = threatLevelTxts[island.landThreat].text;
        landThreatTxt.color = threatLevelTxts[island.landThreat].color;

        navalThreatTxt.text = threatLevelTxts[island.navalThreat].text;
        navalThreatTxt.color = threatLevelTxts[island.navalThreat].color;

        for (int i = 0; i <= island.difficulty; i++)
        {
            if (island.difficulty == 0) dots[i].color = Color.green;
            if (island.difficulty == 1) dots[i].color = Color.yellow;
            if (island.difficulty == 2) dots[i].color = Color.red;
        }
        _report.SetActive(true);
    }

    public void HideReport()
    {
        foreach(Image dot in dots)
        {
            dot.color = _dotColor;
        }
        _report.SetActive(false);
    }

    public void PrepareOperation()
    {
        _island.Attack();
    }
}
