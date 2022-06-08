using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public float currScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        currScore = 0;
    }

    // Update is called once per frame
    public void AddScore(float amount){
        currScore+=amount;
    }

    void Update(){
        scoreText.text = currScore.ToString();
    }
}
