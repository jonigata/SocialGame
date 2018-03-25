using System;
using UnityEngine;

public class MockUpToolBase : Router {
    [SerializeField] int priority;

    protected void DisableIfNotHighestPriority<T>() where T: MockUpToolBase {
        var a = FindObjectsOfType<T>();
        foreach (var e in a) {
            if (e.priority < priority) {
                gameObject.SetActive(false);
            }
        }
    }
}
