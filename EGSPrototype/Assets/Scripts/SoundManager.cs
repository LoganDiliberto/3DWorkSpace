using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{

    public enum Sound {
        PlayerMove,
        PlayerMeleeAttack,
        PlayerRangedAttack,
        EnemyHit,
        EnemyDie,
        PlayerTakeDamage,
    }
    public static void PlaySound(){
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        //audioSource.PlayOneShot();//Add reference to the sound to play
    }
}
