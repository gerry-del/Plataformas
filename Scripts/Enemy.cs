using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    public float movHor = 0f;
    public float speed = 3f;

    public bool isGroundFloor = true;
    public bool isGroundFront = false;

    public LayerMask groundLayer;
    public float frontGrndRayDist = 0.25f;
    public float floorCheckY = 0.52f;
    public float frontCheck = 0.51f;
    public float frontDist = 0.001f;

    public int scoreGive = 50;

    private RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

            //Evitar caer precipicio
            isGroundFloor = (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - floorCheckY, transform.position.z), new Vector3(movHor, 0, 0), frontGrndRayDist, groundLayer));

            if (isGroundFloor)
                movHor = movHor * -1;

            //choque con pared
            if (Physics2D.Raycast(transform.position, new Vector3(movHor, 0, 0), frontCheck, groundLayer))
                movHor = movHor * -1;

            //choque con otro enemigo
            hit = Physics2D.Raycast(new Vector3(transform.position.x + movHor * frontCheck, transform.position.y, transform.position.z), new Vector3(movHor, 0, 0), frontDist);

            if (hit != null)
                if (hit.transform != null)
                    if (hit.transform.CompareTag("Enemy"))
                        movHor = movHor * -1;
        
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movHor * speed, rb.velocity.y);
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Dañar player
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.obj.getDamage();
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //destruir enemigo
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.obj.playEnemyHit();
            getKilled();
        }

        
    }

    // cuando muera el enemigo
    private void getKilled()
    {
        FXManager.obj.showPop(transform.position);
        gameObject.SetActive(false);
    }
}
