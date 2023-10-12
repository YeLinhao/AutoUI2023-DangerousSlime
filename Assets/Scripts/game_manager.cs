using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



[Serializable]
public class TakeOverTime_Json
{
    public List<float> TimeRecord = new List<float>();

} 



public class game_manager : MonoBehaviour
{
    public Image TakeOverPanel;
    public TMP_Text TakeOverTimeText;
    public TMP_Text TakeOverSuccessText;
    public TMP_Text TotalTime;
    public TMP_Text InputCheck;


    [Header("SetTakeOverTime")]
    public float TakeOverTime1;
    public float TakeOverTime2;
    public float TakeOverTime3;
    public float TakeOverTime4;
    public float TakeOverTime5;
    public float TakeOverTime6;
    public float TakeOverTime7;
    public float TakeOverTime8;
    public float TakeOverTime9;
    public float TakeOverTime10;

    

    [Header("Timer")]
    public static float TotalTimer = 0;
    public float TakeOverTimer = 0;
    public int CurrentTakeOverIndex = 0;//记录当前第几次接管


    public static bool IsTakeOver = false;

    TakeOverTime_Json timeRecord;
    
    // Start is called before the first frame update
    void Start()
    {
       timeRecord = new TakeOverTime_Json();
    }

    // Update is called once per frame
    void Update()
    {
        int TakeOverPressed = 0;
        #region 罗技输入 LogitechInput
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            InputCheck.text = "InputCheck:True";

            //手柄四键＋接管键
            for (int i = 0; i < 128; i++)
            {
                if (rec.rgbButtons[i] == 128)
                {

                    if (i == 23)//接管键
                    {
                        TakeOverPressed = 1;
                    }


                }

            }
        }
        #endregion


        TotalTime.text = TotalTimer.ToString("#0.00s");

        //到达接管时间 IfTakeOverTimeReached
        //I'm sorry for ugly code,But it works :)
        if ((Mathf.Abs(TotalTimer - TakeOverTime1) < 0.3f || Mathf.Abs(TotalTimer - TakeOverTime2) <0.3f || Mathf.Abs(TotalTimer - TakeOverTime3) < 0.3f || Mathf.Abs(TotalTimer - TakeOverTime4) < 0.3f) || Mathf.Abs(TotalTimer - TakeOverTime5) < 0.3f || Mathf.Abs(TotalTimer - TakeOverTime6) < 0.3f || Mathf.Abs(TotalTimer - TakeOverTime7) < 0.3f || Mathf.Abs(TotalTimer - TakeOverTime8) < 0.3f || Mathf.Abs(TotalTimer - TakeOverTime9) < 0.3f || Mathf.Abs(TotalTimer - TakeOverTime10) < 0.3f && IsTakeOver == false)
        {
            TakeOverPanel.gameObject.SetActive(true);
            IsTakeOver = true;
        }

        //若正在接管
        if (IsTakeOver == true)
        {
            TakeOverTimer += Time.deltaTime;
           
            #region 方向盘版接管 TakeOverBySteeringWheel
            if (TakeOverPressed == 1)
            {
                IsTakeOver = false;
                TakeOverSuccessText.gameObject.SetActive(true);

                StartCoroutine(TakeOverVanish());
            }
            #endregion 

            #region 键鼠版接管 TakeOverByKeyBoard
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsTakeOver = false;
                TakeOverSuccessText.gameObject.SetActive(true);

                StartCoroutine(TakeOverVanish());
            }
            #endregion 

        }

        if (IsTakeOver == false)
        {
            
           
        }
        TotalTimer += Time.deltaTime;

        TakeOverTimeText.text = TakeOverTimer.ToString("Takeover Time：#0.00s");

    }

    IEnumerator TakeOverVanish()
    {
        
        yield return new WaitForSeconds(2.5f);
        TakeOverSuccess(TakeOverTimer);
        TakeOverTimer = 0;
    }


    public void TakeOverSuccess(float TakeOverTime)
    {

        TakeOverPanel.gameObject.SetActive(false);
        TakeOverSuccessText.gameObject.SetActive(false);
        
        CurrentTakeOverIndex++;
        timeRecord.TimeRecord.Add(TakeOverTime);
        if (CurrentTakeOverIndex == 10)
        {
            string strJson = JsonUtility.ToJson(timeRecord, true);
#if UNITY_EDITOR 
            string jsonPath = "Assets/Data/TakeOverTime.json";

#else
            string jsonPath = Application.dataPath + "/TakeOverTime.json";
#endif
            File.Create(jsonPath).Dispose();
            File.WriteAllText(jsonPath, strJson);

        }

        if (TakeOverTime < 0.3f)
        {
            GameEvents.current.MoneyChange(200);
        }
        else if (TakeOverTime < 0.7f)
        {
            GameEvents.current.MoneyChange(150);
        }
        else if (TakeOverTime < 1f)
        {
            GameEvents.current.MoneyChange(100);
        }
        else if (TakeOverTime < 1.5f)
        {
            GameEvents.current.MoneyChange(50);
        }
        else if (TakeOverTime < 2f)
        {
            GameEvents.current.MoneyChange(0);
        }
    }


    public void GameOver()
    {
        
 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
