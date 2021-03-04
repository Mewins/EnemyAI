using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
  void TakeHeal(int value)
  {
        //Player.Hp += value;
        Destroy(gameObject);
  }
}
