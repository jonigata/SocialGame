using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

public class TopLevelRouter : Router {
    [SerializeField] string moveTo;

    static TopLevelRouter self;
    static ArraySegment<string> prevPath;

    public static TopLevelRouter Instance { get { return self; } }

    void Awake() {
        Assert.IsNull(self);
        self = this;
    }

    void OnDestroy() {
        Assert.IsTrue(self == this);
        self = null;
    }

    public static void MoveTo(string path) {
        var currPath = path.Split('/');

        int keep = 0;
        if (prevPath.Array != null) {
            for (int  i = 0 ; i < currPath.Length ; i++) {
                if (currPath[i] != prevPath.Array[prevPath.Offset + i]) {
                    break;
                }
                keep++;
            }

            var prevPlan = new Plan();
            prevPlan.path = prevPath;
            prevPlan.keep = keep;
            self.leave.OnNext(prevPlan);
        }

        var currPlan = new Plan();
        currPlan.path = new ArraySegment<string>(currPath, 0, currPath.Length);
        currPlan.keep = keep;
        prevPath = currPlan.path;
        self.enter.OnNext(currPlan);
    }

    void Start() {
        MoveTo(moveTo);
    }

    public static bool IsAvailable() {
        return self != null;
    }

}
