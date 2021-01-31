using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{   
    public int maxHealth;
    public int damage;
    public float knockbackForce;
    public Vector2 knockbackDir;
    public Vector3 hitPointOffset;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void Die()
    {   
        Destroy(gameObject, 0.1f);
    }
    public void GetHit(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + hitPointOffset, Color.green);
    }
    
    //Damaging hitbox
    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        if(other.CompareTag("playerDamageBox"))
        {
            GetHit(1);
        }
    }

    //Hit player
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log("Collide with " + other.tag);
        if(other.CompareTag("Player"))
        {
            //Player hit on head?
            PlayerPhysics physics = other.GetComponent<PlayerPhysics>();
            if((physics.transform.position + physics.hitPointOffset).y > (transform.position + hitPointOffset).y && physics.IsRoll())
            {
                Debug.Log("Hit");
                GetHit(1);
            }//Else do damage
            else if(!physics.p.IsInvincible())
            {
                Debug.Log("Take damage");
                physics.p.TakeDamage(damage);
                physics.Knockback(new Vector2(knockbackDir.x * Mathf.Sign(physics.transform.position.x - transform.position.x), knockbackDir.y), knockbackForce);
                physics.p.SetTempInvincible();
            }
        }
    }
    
}
