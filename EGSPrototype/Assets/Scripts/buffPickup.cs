using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffPickup : MonoBehaviour
{
    public Transform player;
    public Player playerObj;
    public LayerMask whatIsGround, whatIsPlayer;

    public GameObject gameMaster;
    public Component waveSpawner;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        playerObj = GameObject.Find("Player").GetComponent<Player>();
        gameMaster = GameObject.Find("GameMaster");
        //waveSpawner = GameObject.Find("GameMaster").GetComponent<WaveSpawner>;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(playerObj.GetComponent<Player>().isBuffed == false)
            {
                playerObj.GetComponent<Player>().isBuffed = true;
                Destroy(this.gameObject);
                gameMaster.GetComponent<WaveSpawner>().buffCount -= 1;
            }
        }
        else
        {
            //Destroy(this.gameObject);
        }



    }


}
