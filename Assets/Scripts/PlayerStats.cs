using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int Health;
    private int MaxHealth = 100;
    public int Damage = 20;
    public GameObject StartPos, DeathPos;
    public Slider slider;

    private void Start()
    {
        Health = MaxHealth;
        StartPos = GameObject.Find("StartPos");
        DeathPos = GameObject.Find("DeathPos");
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            DestroyPlayer();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Health = -9999;
        }
        if (Health <= 0)
        {
            gameObject.transform.position = DeathPos.transform.position;
            Health = 1;
        }

        slider.value = Health;
    }
    void DestroyPlayer()
    {
        gameObject.transform.position = DeathPos.transform.position;
        Health = MaxHealth;
    }


}
