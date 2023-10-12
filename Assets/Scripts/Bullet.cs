using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0f;
    public float moveAngle;
    private Rigidbody2D body;
    public int damageNumber;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = transform.position + Time.deltaTime * speed * (transform.right * Mathf.Cos(moveAngle) + transform.up * Mathf.Sin(moveAngle));
        //body.MovePosition(transform.position + Time.deltaTime * speed * (transform.right * Mathf.Cos(moveAngle) + transform.up * Mathf.Sin(moveAngle)));

        if (this.transform.position.x >= 20 || this.transform.position.x <= -20 || this.transform.position.y >= 10 || this.transform.position.y <= -10)
        {
            GameObject.Destroy(gameObject);
        }
        
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameEvents.current.MoneyChange(-1*damageNumber);
            //GameObject.Destroy(collision.gameObject);
            GameObject.Destroy(gameObject);

        }

        if (collision.gameObject.tag == "Sheild")
        {
            GameEvents.current.SheildHealthChange(-1*damageNumber);
            GameObject.Destroy(gameObject);
        }

    }


}
