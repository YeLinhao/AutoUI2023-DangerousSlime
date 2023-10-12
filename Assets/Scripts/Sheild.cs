using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Sheild : MonoBehaviour
{
    public int SheildHealth = 25;
    public Slider SheildBar;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onSheildHealthChange += SheildHealthChange;
    }

    // Update is called once per frame
    void Update()
    {
        int RotateClockwisePressed = 0;
        int RotateCounterClockwisePressed = 0;
        #region LogitechInput_Shield
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);


            //方向键输入
            switch (rec.rgdwPOV[0])
            {
                case (9000): RotateClockwisePressed = 1; break;//按 PressRightBtn
                case (27000): RotateCounterClockwisePressed = 1; break;// PressLeftBtn
                default: ; break;
            }
        }
        #endregion


        SheildBar.value = SheildHealth;
        if (SheildHealth <= 0)
        {
            GameObject.Destroy(gameObject);
        }

        if (RotateClockwisePressed == 1)
        {
            this.transform.Rotate(0, 0, 1, Space.World); 
        }
        if (RotateCounterClockwisePressed == 1)
        {
            this.transform.Rotate(0, 0,-1, Space.World);
        }
    }

    public void SheildHealthChange(int changeQuantity)
    {
        if (game_manager.IsTakeOver == false)
        {
            SheildHealth += changeQuantity;
        }
        
    }

}
