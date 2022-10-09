using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CharacterData_", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public float health;
    public float maxHealth;
    public float damage;
    public float damageForce;
    public float force;
    public float maxForce;
}
