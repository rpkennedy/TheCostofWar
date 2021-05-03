using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public enum CellType
{
    water,
    land,
    obstacle,
    safeZone
}
public class Cell : MonoBehaviour
{
    public Operation op;
    private int x;
    private int y;

    public CellType type;

    public bool hasUnit;
    public bool hasEnemy;

    public bool isFogged = true;
    bool permaUnfogged = false;
    public bool isSmoked = false;

    private int smokeDuration;

    public GameObject fog;
    public GameObject smoke;
    public GameObject explosion;

    public Button button;
    private Color defaultButtonColor;
    private Color defaultSelectedButtonColor;

    public GameObject unit;
    public GameObject enemy;

    private void Awake()
    {
        op = GameObject.FindGameObjectWithTag("Operation").GetComponent<Operation>();
        button = this.gameObject.GetComponent<Button>();
        defaultButtonColor = button.colors.normalColor;

        fog = this.gameObject.transform.GetChild(0).gameObject;
        smoke = this.gameObject.transform.GetChild(1).gameObject;
        explosion = this.gameObject.transform.GetChild(2).gameObject;

        if (hasEnemy)
        {
            enemy.GetComponent<SpriteRenderer>().enabled = false;
        }        
    }

    void Start()
    {
        CheckFogLogic();
    }

    public void BomberSmoke()
    {
        smokeDuration = 2;

        smoke.SetActive(true);
        isSmoked = true;
    }
    public void SmokeClearing()
    {
        smokeDuration--;

        if(smokeDuration <= 0)
        {
            isSmoked = false;
            smoke.SetActive(false);
        }
    }

    public void BomberBomb(int damage)
    {
        explosion.SetActive(true);

        if (hasEnemy) enemy.GetComponent<Enemy>().TakeDamage(damage);
        if (hasUnit)
        {
            if (unit.name.Contains("Platoon")) unit.GetComponent<Platoon>().TakeDamage(damage);
            if (unit.name.Contains("ship")) unit.GetComponent<Battleship>().TakeDamage(damage);
            if (unit.name.Contains("Carrier")) unit.GetComponent<Carrier>().TakeDamage(damage);
        }
        Invoke("ClearExplosion", 0.5f);
    }
    private void ClearExplosion()
    {
        explosion.SetActive(false);
    }

    public void CheckFogLogic()
    {
        List<Cell> immediateCells = op.GetNeighborCells(this.gameObject.GetComponent<Cell>());
        immediateCells.Add(this.gameObject.GetComponent<Cell>());
        
        if (hasUnit)
        {
            foreach (Cell cell in immediateCells)
            {
                cell.DisableFog();
            }            
        }        
        else if(type == CellType.safeZone)
        {
            DisableFog();
        }
        else if(!permaUnfogged)
        {
            EnableFog();
        }
    }

    public void DisableFog()
    {
        permaUnfogged = true;
        isFogged = false;
        fog.SetActive(false);

        if (hasEnemy)
        {
            enemy.GetComponent<SpriteRenderer>().enabled = true;
        }        
    }

    public void DisableFogRecon()
    {
        isFogged = false;
        fog.SetActive(false);

        if (hasEnemy)
        {
            enemy.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void EnableFog()
    {
        isFogged = true;
        fog.SetActive(true);
        if (hasEnemy)
        {
            enemy.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void ButtonInput()
    {
        //set deployer on op, relay this cell to deployer through op when bombing
        if (op.bombing)
        {
            op.SetBomberTarget(this);
            op.bombing = false;
        }

        if(op.pathfinder.deployUnit != null)
        {
            if(type != CellType.obstacle)
            {
                op.pathfinder.SetTargetCell(this);
            }
            else
            {
                Debug.Log("Obstacle");
                op.pathfinder.deployUnit = null;
            }
            
        }
        if (op.moveInProgress)
        {
            if (type != CellType.obstacle)
            {
                op.pathfinder.SetTargetCell(this);
            }
            else
            {
                Debug.Log("Obstacle");
                op.moveInProgress = false;
                op.pathfinder.startCell = null;
            }
        }
        else if (unit != null && !op.pathfinder.GetAttacking())
        {
            Debug.Log("Button Inp 2");
            TriggerUnitAbilityMenu();
        }
        else if (op.pathfinder.GetAttacking())
        {
            if (type != CellType.obstacle)
            {
                op.pathfinder.SetTargetCell(this);
            }
            else
            {
                Debug.Log("Obstacle");
                op.pathfinder.startCell = null;
                op.pathfinder.StopAttacking();
            }
        }
        else if(unit == null && GameObject.FindGameObjectWithTag("abilityPanel") != null)
        {
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("abilityPanel"))
            {
                Destroy(go);
            }
        }
    }

    public void TriggerUnitAbilityMenu()
    {
        if (GameObject.FindGameObjectWithTag("abilityPanel") != null)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("abilityPanel"))
            {
                Destroy(go);
            }
        }
        if (!unit.GetComponent<Unit>().HasAP()) return;
        Debug.Log("made it to trigger");
        op.pathfinder.startCell = this;
        op.pathfinder.refCell = this;
        unit.GetComponent<Unit>().ShowAbilities();
    }

    public void AddEnemy(GameObject newEnemy) 
    {
        enemy = newEnemy;
        hasEnemy = true;
        enemy.transform.position = this.transform.position;
    }

    public void RemoveEnemy()
    {
        enemy.SetActive(false);
        enemy = null;
        hasEnemy = false;
    }

    public int GetX()
    {
        return x;
    }
    public int GetY()
    {
        return y;
    }
    public void SetX(int i)
    {
        x = i;
    }
    public void SetY(int i)
    {
        y = i;
    }

    public void SetUnit(GameObject newUnit)
    {
        unit = newUnit;
        hasUnit = true;
        unit.transform.position = this.transform.position;
    }

    public void SetButtonRed()
    {
        var colors = button.colors;
        colors.normalColor = new Color(1, 0, 0, 1);
        button.colors = colors;
        Invoke("SetButtonDefault", 1f);
    }
    public void SetButtonGreen()
    {
        var colors = button.colors;
        colors.normalColor = new Color(0, 1, 0, 1);
        button.colors = colors;
        Invoke("SetButtonDefault", 1f);
    }
    public void SetButtonDefault()
    {
        var colors = button.colors;
        colors.normalColor = defaultButtonColor;
        button.colors = colors;
    }

    public void SetTargetButtonRed()
    {
        var colors = button.colors;
        colors.selectedColor = new Color(1, 0, 0, 1);
        button.colors = colors;
        Invoke("SetTargetButtonDefault", 1f);
    }
    public void SetTargetButtonGreen()
    {
        var colors = button.colors;
        colors.selectedColor = new Color(0, 1, 0, 1);
        button.colors = colors;
        Invoke("SetTargetButtonDefault", 1f);
    }
    public void SetTargetButtonDefault()
    {
        var colors = button.colors;
        colors.selectedColor = defaultSelectedButtonColor;
        button.colors = colors;
    }
}
