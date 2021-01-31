using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{   
    public int maxHealth;
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        Debug.Log(other.tag);
        if(other.CompareTag("playerDamageBox"))
        {
            Debug.Log("collide");
            if(other.transform.position.y > transform.position.y)
            {
                GetHit(1);
            }
        }
    }
    
}
