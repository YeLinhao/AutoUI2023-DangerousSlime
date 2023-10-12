using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x >= 20 || this.transform.position.x <= -20 || this.transform.position.y >= 10 || this.transform.position.y <= -10)
        {
            GameObject.Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy_Small")
        {

           
            GameObject.Destroy(gameObject);
            GameEvents.current.ShootSlime(1);
            GameEvents.current.MoneyChange(5);
        }

        if (collision.gameObject.tag == "Enemy_Middle")
        {

            
            GameObject.Destroy(gameObject);
            GameEvents.current.ShootSlime(2);
            GameEvents.current.MoneyChange(10);
        }

        if (collision.gameObject.tag == "Enemy_Large")
        {
            
           // GameObject.Destroy(collision.gameObject);
            GameObject.Destroy(gameObject);
            GameEvents.current.ShootSlime(3);
            GameEvents.current.MoneyChange(20);
        }



    }


}