using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    private bool awake = false;

    private float timeToDestroy = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        awake = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (awake)
        {
            timeToDestroy -= Time.deltaTime;
        }

        if (timeToDestroy <= 0f)
        {
            Destroy(this.gameObject);
            awake = false;
            timeToDestroy = 2f;
        }
    }
}
