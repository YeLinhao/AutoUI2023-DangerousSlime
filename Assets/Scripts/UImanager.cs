using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    
  
    public void Scene_ChangeToPureTakeOver()
    {
        SceneManager.LoadScene("Pure_TakeOver");
    }
    public void Scene_ChangeToMovie()
    {
        SceneManager.LoadScene("Movie");
    }
    public void Scene_ChangeToAttack()
    {
        SceneManager.LoadScene("Attack");
    }

    public void Scene_ChangeToDefend()
    {
        SceneManager.LoadScene("Defend");
    }


    void Update()
    {
   
    }

}
