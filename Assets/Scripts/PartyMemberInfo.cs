using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Party Member")]
public class PartyMemberInfo : ScriptableObject
{
    public string MemberName;
    public int StartingLevel;
    public int BaseHealth;
    public int BaseStr; //Str = Strength
    public int BaseSpd; //Spd = Speed
    public GameObject MemberBattleVisualPrefab; // What is visible during battle scene
    public GameObject MemberOverworldVisualPrefab; // What is visible during the overworld scene
}
