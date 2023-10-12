using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] float shootTimer = 3f;
    float timer = 0f;
    [SerializeField] GameObject bullet;

    /// <summary>
    /// 发射角为多少个派
    /// </summary>
    [SerializeField] float shootAngle_start = 0;
    [SerializeField] float shootAngle_end = 0;
    [SerializeField] float shootNumber = 0;
    private float PI = 3.1415926f;

    // Start is called before the first frame update
    void Start()
    {
        shootAngle_start *= PI;
        shootAngle_end *= PI;
    }

    // Update is called once per frame
    void Update()
    {
        #region 罗技输入
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            //方向键输入
            switch (rec.rgdwPOV[0])
            {
                case (0): Debug.Log("方向键: 上"); break;
                case (4500): Debug.Log("方向键: 右上"); break;
                case (9000): Debug.Log("方向键: 右"); break;
                case (13500): Debug.Log("方向键: 右下"); break;
                case (18000): Debug.Log("方向键: 下"); break;
                case (22500): Debug.Log("方向键: 左下"); break;
                case (27000): Debug.Log("方向键: 左"); break;
                case (31500): Debug.Log("方向键: 左上"); break;
                default: Debug.Log("方向键: 中心"); break;
            }


            //手柄四键＋接管键
            for (int i = 0; i < 128; i++)
            {
                if (rec.rgbButtons[i] == 128)
                {
                    if (i == 0)
                    {
                        Debug.Log("按下X键！");
                    }

                    if (i == 1)
                    {
                        Debug.Log("按下方块键！");
                    }

                    if (i == 2)
                    {
                        Debug.Log("按下圆圈键！");
                    }
                    if (i == 3)
                    {
                        Debug.Log("按下三角键！");
                    }


                    if (i == 23)
                    {
                        Debug.Log("按下回车接管键！");
                    }


                }

            }
        }

        #endregion
        if (timer > shootTimer)
        {
            Bullet bulletcomp = bullet.GetComponent<Bullet>();
            GameObject Player = GameObject.FindGameObjectWithTag("Player");

            float shootAngle = Mathf.Atan((Player.transform.position.y - gameObject.transform.position.y) / (Player.transform.position.x - gameObject.transform.position.x));
            if (gameObject.transform.position.x > Player.transform.position.x)
            {
                shootAngle += 3.1416f;
            }

            bulletcomp.moveAngle = shootAngle;
            
            Instantiate(bullet, transform.position , Quaternion.identity);
            timer = 0f;

        }
        timer += Time.deltaTime;
    }
}
