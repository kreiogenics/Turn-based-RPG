using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 0)]
public class Quest : ScriptableObject {
    [SerializeField]
    string[] tasks;

    public IEnumerable<string> GetTask()
    {
        yield return "Task 1";
    }
}