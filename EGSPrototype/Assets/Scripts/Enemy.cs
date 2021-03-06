using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 100;
    int currentHealth;
    ScoreController scoreController;

    // Start is called before the first frame update
    void Start()
    {
        //Start with max health
        currentHealth = maxHealth;
        //scoreController = GameObject.FindGameObjectWithTag("ScoreController").GetComponent<ScoreController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        //Play hurt animation

        //Take Damage
        currentHealth -= damage;
        
        //Check if dead
        if(currentHealth <= 0)
            Die();
    }

    void Die(){

        //Add death animation
        Debug.Log("Enemy Died");
        Destroy(gameObject);
        scoreController.AddScore(10);
        
    }
}
