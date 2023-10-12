using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] float shootTimer = 3f;
    float timer = 0f;
    [SerializeField] GameObject bullet;

    /// <summary>
    /// �����Ϊ���ٸ���
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
        #region �޼�����
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            //���������
            switch (rec.rgdwPOV[0])
            {
                case (0): Debug.Log("�����: ��"); break;
                case (4500): Debug.Log("�����: ����"); break;
                case (9000): Debug.Log("�����: ��"); break;
                case (13500): Debug.Log("�����: ����"); break;
                case (18000): Debug.Log("�����: ��"); break;
                case (22500): Debug.Log("�����: ����"); break;
                case (27000): Debug.Log("�����: ��"); break;
                case (31500): Debug.Log("�����: ����"); break;
                default: Debug.Log("�����: ����"); break;
            }


            //�ֱ��ļ����ӹܼ�
            for (int i = 0; i < 128; i++)
            {
                if (rec.rgbButtons[i] == 128)
                {
                    if (i == 0)
                    {
                        Debug.Log("����X����");
                    }

                    if (i == 1)
                    {
                        Debug.Log("���·������");
                    }

                    if (i == 2)
                    {
                        Debug.Log("����ԲȦ����");
                    }
                    if (i == 3)
                    {
                        Debug.Log("�������Ǽ���");
                    }


                    if (i == 23)
                    {
                        Debug.Log("���»س��ӹܼ���");
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
