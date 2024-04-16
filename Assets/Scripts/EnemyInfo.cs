using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Enemy")]
public class EnemyInfo : ScriptableObject
{
    public string EnemyName;
    //public int StartingLevel;
    public int BaseHealth;
    public int BaseStr; //Str = Strength
    public int BaseSpd; //Spd = Speed
    public GameObject EnemyVisualPrefab; // What is visible during battle scene
}
