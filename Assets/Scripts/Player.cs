using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class Player : MonoBehaviour
{
    [Header("PlayerProperties")]
    public string type;
    public int Health = 50;
    public float m_speed;
    public int Money = 0;
    public int GainMoneyAmount;
    public int GainMoneyFrequency;
    private float MoneyTimer = 0;
    

    [Header("GunProperties")]
    public float FireRate = 2.0f;
    
    /// <summary>
    /// 当前子弹量
    /// </summary>
    public int BulletAmount_Temp;
    
    /// <summary>
    /// 弹夹容量
    /// </summary>
    public int BulletAmount_Max;
    
    /// <summary>
    /// 子弹对象
    /// </summary>
    public GameObject Bullet;
    /// <summary>
    /// 子弹速度
    /// </summary>
    public float BulletSpeed;

    private float nextFire;
    private bool ReloadState = false;


    [Header("UIControl")]
    public TMP_Text MoneyText;
    public TMP_Text GunText;
    public Slider HealthBar;
    public Image CoinImage;
    public Image ChestImage;
    public Image GameOverPanel;

    private Rigidbody2D m_body2d;
    SpriteRenderer sprite;




    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onPlayerHealthChange += PlayerHealthChange;
        GameEvents.current.onMoneyChange += PlayerMoneyChange; 

        m_body2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();


        Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(false)); //罗技方向盘是否连接
    }

    // Update is called once per frame
    void Update()
    {

        int moveDirection = 0;
        float shootDirection = 0.1f;
       
        #region LogitechInput
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            //方向键输入 Direction Input
            switch (rec.rgdwPOV[0])
            {
                case (0): shootDirection = 0.5f; break;
                case (4500): shootDirection = 0.25f; break;
                case (9000): shootDirection = 0; break;
                case (13500): shootDirection = 1.75f; break;
                case (18000): shootDirection = 1.5f; break;
                case (22500): shootDirection = 1.25f; break;
                case (27000): shootDirection = 1.0f; break;
                case (31500): shootDirection = 0.75f; break;
                default: shootDirection = 0.1f; break;//If shootDirection 
            }

            //方块键、圆圈键输入 Square and Circle Btn Input
            for (int i = 0; i < 128; i++)
            {
                if (rec.rgbButtons[i] == 128)
                {


                    if (i == 1)//方块键 Square Btn
                    {
                        moveDirection = -1;//向左 Left
                    }

                    if (i == 2)//圆圈键 Circle Btn
                    {
                        moveDirection = 1;//向右 Right
                    }

                }

            }
        }

        #endregion


        HealthBar.value = Health;

        if (type != "Defend")
        {
            GunText.text = "Bullet: " + BulletAmount_Temp.ToString() + " / " + BulletAmount_Max.ToString();
        }
        

        #region GainMoney
        MoneyTimer += Time.deltaTime;
        if (MoneyTimer >= GainMoneyFrequency && game_manager.IsTakeOver == false)
        {
            Money += GainMoneyAmount;
            MoneyTimer = 0;
        }
        MoneyText.text ="Money: "+ Money.ToString();
        #endregion
        #region HeroMove

        //float inputX = Input.GetAxis("Horizontal");
        //float inputY = Input.GetAxis("Vertical");

        float inputX = moveDirection;


        if (inputX > 0)
            sprite.flipX = true;
        else if (inputX < 0)
            sprite.flipX = false;

        //if (inputY > 0)
        //    sprite.flipY = true;
        //else if (inputY < 0)
        //    sprite.flipY = false;

        // Move
        m_body2d.velocity = new Vector2(inputX * m_speed, 0 /*inputY * m_speed*/);
        #endregion


        #region PlayerGun
        nextFire += Time.deltaTime;

        #region Fire_Logitech
        if (shootDirection != 0.1f && nextFire > FireRate && BulletAmount_Temp > 0)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.x = Mathf.Cos(shootDirection * 3.14f) *50;
            mousePosition.y = Mathf.Sin(shootDirection * 3.14f) *50;
            mousePosition.z = 0;

            //子弹角度 Bullet Angle
            float fireAngle = Vector2.Angle(mousePosition - this.transform.position, Vector2.up) - 90;


            if (mousePosition.x > this.transform.position.x)
            {
                fireAngle = -fireAngle;
            }
            if (mousePosition.x <= this.transform.position.x)
            {
                fireAngle += 180;
            }

            //计时器归零 TimeReset
            nextFire = 0;

            //生成子弹 BulletSpawn
            GameObject HeroBullet = Instantiate(Bullet, transform.position + (mousePosition - transform.position).normalized, Quaternion.identity) as GameObject;
            BulletAmount_Temp -= 1;

            //速度 Speed
            HeroBullet.GetComponent<Rigidbody2D>().velocity = ((mousePosition - transform.position).normalized * BulletSpeed);

            //角度 Angle
            HeroBullet.transform.localEulerAngles = new Vector3(0, 0, fireAngle);
        }
        #endregion

        #region Fire_PC
        if (Input.GetMouseButton(0) && nextFire > FireRate && BulletAmount_Temp > 0)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0;

            //子弹角度
            float fireAngle = Vector2.Angle(mousePosition - this.transform.position, Vector2.up) - 90;


            if (mousePosition.x > this.transform.position.x)
            {
                fireAngle = -fireAngle;
            }
            if (mousePosition.x <= this.transform.position.x)
            {
                fireAngle += 180;
            }

            //计时器归零
            nextFire = 0;

            //生成子弹
            GameObject HeroBullet = Instantiate(Bullet, transform.position + (mousePosition - transform.position).normalized, Quaternion.identity) as GameObject;
            BulletAmount_Temp -= 1;

            //速度
            HeroBullet.GetComponent<Rigidbody2D>().velocity = ((mousePosition - transform.position).normalized * BulletSpeed);

            //角度
            HeroBullet.transform.localEulerAngles = new Vector3(0, 0, fireAngle);
        }
        #endregion

        if (BulletAmount_Temp == 0 && ReloadState == false)
        {
            ReloadState = true;
            StartCoroutine(Reload());
        }

        IEnumerator Reload()
        {
            Debug.Log("成功！");
            yield return new WaitForSeconds(2.0f);
            BulletAmount_Temp = BulletAmount_Max;
            ReloadState = false;
        }

        #endregion

        if (Health <= 0)
        {
            GameOverPanel.gameObject.SetActive(true);
            Time.timeScale = 0.2f;
        }
    }

    public void PlayerHealthChange(int changeQuantity)
    {
        if (game_manager.IsTakeOver == false)
        {
            Health += changeQuantity;
        }
        
    }

    public void PlayerMoneyChange(int changeQuantity)
    {
       Money += changeQuantity;
        if (changeQuantity >= 100)
        {
            StartCoroutine(MoneyChange_Chest());
        }

        if (changeQuantity < 100 && changeQuantity >= 10)
        {
            StartCoroutine(MoneyChange_Coin());
        }

    }

    IEnumerator MoneyChange_Coin()
    {

        CoinImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        CoinImage.gameObject.SetActive(false);

    }

    IEnumerator MoneyChange_Chest()
    {

        ChestImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        ChestImage.gameObject.SetActive(false);

    }

}
