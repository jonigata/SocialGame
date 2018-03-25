using System;
using UnityEngine;

public class MockUpTool : MockUpToolBase {
    void Awake() {
        DisableIfNotHighestPriority<MockUpTool>();
    }
}
