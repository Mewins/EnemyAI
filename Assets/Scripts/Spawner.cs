using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject EnemyPref, Enemy;
    public OpenDoor openDoor;
    private int hp = 10;
    public int spawnTime=2;

    void TakeDamage(int damage = 1)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            Enemy = Instantiate(EnemyPref);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

}
