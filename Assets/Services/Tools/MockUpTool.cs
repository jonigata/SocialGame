using System;
using UnityEngine;

public class MockUpTool : Router {
    [SerializeField] int priority;

    void Awake() {
        var a = FindObjectsOfType<MockUpTool>();
        foreach (var e in a) {
            if (e.priority < priority) {
                gameObject.SetActive(false);
            }
        }
    }
}
