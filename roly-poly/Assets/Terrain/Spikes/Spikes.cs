using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float knockbackForce;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerPhysics physics = collision.gameObject.GetComponent<PlayerPhysics>();
            if(!physics.p.IsInvincible())
            {
                physics.p.TakeDamage(1);
                physics.Knockback(new Vector2(Mathf.Sign(physics.transform.position.x - collision.transform.position.x), 1), knockbackForce);
                physics.p.SetTempInvincible();
            }

        }   
    }
}
