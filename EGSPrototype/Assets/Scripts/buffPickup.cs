using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffPickup : MonoBehaviour
{
    public Transform player;
    public Player playerObj;
    public LayerMask whatIsGround, whatIsPlayer;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        playerObj = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(playerObj.GetComponent<Player>().isBuffed == false)
            {
                playerObj.GetComponent<Player>().isBuffed = true;
                Destroy(this.gameObject);
            }
        }
        else
        {
            //Destroy(this.gameObject);
        }



    }


}
