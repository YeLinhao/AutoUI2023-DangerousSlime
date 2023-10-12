using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<int> onPlayerHealthChange;
    public void PlayerHealthChange(int changeQuantity)
    {
        if (onPlayerHealthChange != null)
        {
            onPlayerHealthChange(changeQuantity);
        }
    }

    public event Action<int> onSheildHealthChange;
    public void SheildHealthChange(int changeQuantity)
    {
        if (onSheildHealthChange != null)
        {
            onSheildHealthChange(changeQuantity);
        }
    }

    public event Action<int> onMoneyChange;
    public void MoneyChange(int changeQuantity)
    {
        if (onMoneyChange != null)
        {
            onMoneyChange(changeQuantity);
        }
    }

    public event Action<int> onShootSlime;
    public void ShootSlime(int typeOfSlime)
    {
        if (onShootSlime != null)
        {
            onShootSlime(typeOfSlime);
        }
    }

  


    //public event Action<float> onTakeOverSuccess;
    //public void TakeOverSuccess(float TakeOverTime)
    //{
    //    if (onSheildHealthChange != null)
    //    {
    //        onTakeOverSuccess(TakeOverTime);
    //    }
    //}



}
