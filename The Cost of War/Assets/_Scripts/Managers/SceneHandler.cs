using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    public void LoadScene()
    {        
        SceneManager.LoadScene(DataStore.AttackingIslandID);        
    }
    public void LoadCampaign()
    {
        SceneManager.LoadScene(0);
    }
}
