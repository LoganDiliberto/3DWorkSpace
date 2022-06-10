using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Component collision;

    public Transform player;
    public Player playerObj;
    public LayerMask whatIsGround, whatIsPlayer;

    public GameObject hit;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        playerObj = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            GameObject projectileHit = Instantiate(hit, transform.position, Quaternion.identity);
            playerObj.takeDamage(1);
        }
        else
        {
            //Destroy(this.gameObject);
        }

        

    }
}
