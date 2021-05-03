using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Deployer : MonoBehaviour
{
    private Carrier carrier;
    private Operation op;

    public float planeVelocity;

    public Button platoonButton;
    public Button reconButton;
    public Button bomberButton;

    Platoon plat;

    public void DeployPlatoon()
    {
        plat = carrier.platoonsStored[carrier.platoonsStored.Count -1];
        op.pathfinder.deployUnit = plat.gameObject.GetComponent<Unit>();     
    }

    public void DeployReconPlane()
    {
        if (!carrier.HasAP()) return;
        if (carrier.VerifyAction())
        {
            GameObject recon = carrier.reconPlanesStored[carrier.reconPlanesStored.Count - 1].gameObject;

            recon.transform.position = new Vector3(carrier.gameObject.transform.position.x, carrier.gameObject.transform.position.y);
            recon.GetComponent<ReconPlane>().aboveCell = op.GetCell(this.transform.position);
            recon.GetComponent<SpriteRenderer>().enabled = true;

            //reveal enemies
            recon.GetComponent<ReconPlane>().Recon();

            //op.ReconPlaneFogReveal();

            Destroy(this.gameObject);

            
        }
    }

    public void StartBombing()
    {
        if (!carrier.HasAP()) return;
        if (carrier.VerifyAction())
        {
            op.bombing = true;
        }
    }

    public void DeployBomber(Cell bomberTarget)
    {
        GameObject bomber = carrier.bombersStored[carrier.bombersStored.Count - 1].gameObject;

        bomber.transform.position = new Vector3(bomberTarget.transform.position.x, -op.grid.canvasHeight / 2 - 65);
        bomber.GetComponent<Bomber>().aboveCell = op.cells[bomberTarget.GetX(),0];
        bomber.GetComponent<SpriteRenderer>().enabled = true;
        bomber.GetComponent<Bomber>().Flyby(bomberTarget);

        Destroy(this.gameObject);
    }

    public void Success()
    {
        Debug.Log("Success");
        plat.EnableSprites();
        carrier.platoonsStored.Remove(plat);
        End();
    }

    public void End()
    {
        this.gameObject.SetActive(false);
    }

    public void ConfigureDeployPanel()
    {
        op = GameObject.FindGameObjectWithTag("Operation").GetComponent<Operation>();
        carrier = op.pathfinder.refCell.unit.GetComponent<Carrier>();
        platoonButton.interactable = false;
        reconButton.interactable = false;
        bomberButton.interactable = false;
        if (carrier.platoonsStored.Count > 0) platoonButton.interactable = true;
        if (carrier.reconPlanesStored.Count > 0) reconButton.interactable = true;
        if (carrier.bombersStored.Count > 0) bomberButton.interactable = true;
        op.SetDeployer(this);
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
