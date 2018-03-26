using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

public class TopLevelRouter : MockUpToolBase {
    [SerializeField] Router concreteRouter;

    static TopLevelRouter self;
    static ArraySegment<string> prevPath;

    void Awake() {
        DisableIfNotHighestPriority<TopLevelRouter>();
        if (gameObject.activeSelf) {
            Debug.Log(gameObject.scene.name);
            Debug.Log(gameObject.name);
            Assert.IsNull(self);
            self = this;
        }
    }

    void OnDestroy() {
        if (self == this) {
            self = null;
        }
    }

    public static void JumpTo(string path) {
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
            self.concreteRouter.leave.OnNext(prevPlan);
        }

        var currPlan = new Plan();
        currPlan.path = new ArraySegment<string>(currPath, 0, currPath.Length);
        currPlan.keep = keep;
        prevPath = currPlan.path;
        self.concreteRouter.enter.OnNext(currPlan);
    }

    public static void Mount(string path, Router router) {
        var mount = new Mount();
        var currPath = path.Split('/');
        mount.path = new ArraySegment<string>(currPath, 0, currPath.Length);
        mount.router = router;
        self.concreteRouter.mount.OnNext(mount);
    }
}
