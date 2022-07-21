using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState { SPAWNING, WAITING, COUNTING}
    
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;//Number of enemies
        public float rate;//Spawn Rate

    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCountdown = 1f;//Amount of time between searching if all enemies are dead(taxing method)
    private SpawnState state = SpawnState.COUNTING;

    public GameObject buff;
    public int buffCount = 0;

    private int buffPlaceX;
    private int buffPlaceZ;
    public GameObject buffLocator;
    private float buffPlacementTimer = 0f;
    


    void Start() {
        waveCountdown = timeBetweenWaves;
        if(spawnPoints.Length == 0){
            Debug.LogError("No Spawn Points Referenced");
        }
    }

    void Update(){

        if(state == SpawnState.WAITING){
            //Check if all enemies are dead
            if(!EnemyIsAlive()){
                //Begin a new round
                WaveCompleted();
            }
            else{
                return;
            }
        }

        if(waveCountdown <=0 ){
            if(state != SpawnState.SPAWNING){
                //Start spawning wave
                StartCoroutine( SpawnWave (waves[nextWave]));
            }
        }
        else{
            waveCountdown -= Time.deltaTime;
        }

        if (buffCount <= 4)
        {
            buffPlaceX = Random.Range(-23, 23);
            buffPlaceZ = Random.Range(-23, 12);

            buffLocator.transform.position = new Vector3(buffPlaceX, 1, buffPlaceZ);

            PlaceBuffs();
        }
        //else if (buffPlacementTimer > 0f)
        //{
            //PlaceBuffTimer();
        //}



    }

    void WaveCompleted(){
        Debug.Log("Wave Completed!");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
    
        if(nextWave + 1 > waves.Length - 1 ){
            //If next wave is bigger than the number of waves we have
            nextWave = 0;
            Debug.Log("All Waves Compelte! Looping....");
        }
        else{
            nextWave++;
        }
        
    }
    bool EnemyIsAlive(){
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0){

            searchCountdown = 1f;
            if(GameObject.FindGameObjectWithTag("Enemy") == null){
                return false;//All enemies dead
            }
            else{
                return true;
            }
        }
        //This is a taxing method so lets do it only once a second
        return true;
    }

    IEnumerator SpawnWave(Wave _wave){

        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for(int i = 0; i < _wave.count; i++){
            SpawnEnemy(_wave.enemy); 
            yield return new WaitForSeconds( 1f/_wave.rate);
        }
        
        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy( Transform _enemy){
        Debug.Log("Spawning enemy: " + _enemy.name);

        

        Transform _sp = spawnPoints[ Random.Range(0, spawnPoints.Length)];
        Instantiate (_enemy, _sp.position, _sp.rotation);
    }

    void PlaceBuffs()
    {
        if(buffCount < 4 && buffPlacementTimer <= 0f)
        {
            Transform bP = buffLocator.transform;

            Instantiate(buff, bP);

            buffCount += 1;
            buffPlacementTimer = 20f;
        }

        if (buffPlacementTimer > 0f)
        {
            buffPlacementTimer -= Time.deltaTime;
        }
    }

}
