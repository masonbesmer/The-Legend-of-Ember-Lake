using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public void Heal(float heal);
    public void TakeDamage(int damage);
}
