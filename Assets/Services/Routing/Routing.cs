using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

public class Routing : MockUpToolBase {
    [SerializeField] Router concreteRouter;

    static Routing self;
    static ArraySegment<string> prevPath;

    void Awake() {
        DisableUnlessHighestPriority<Routing>();
        if (gameObject.activeSelf) {
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
        if (path.Length == 0) {
            throw new ArgumentException("Can't jump with null path");
        }
        if (path[0] != '/') {
            throw new ArgumentException("Path must start with '/'");
        }
        path = path.Substring(1);

        Debug.Log("Routing.JumpTo: " + path);
        var currPath = path.Split('/');

        int keep = 0;
        if (prevPath.Array != null) {
            Debug.Log(prevPath.Array.Length);
            Debug.Log(prevPath.Offset);
            Debug.Log(currPath.Length);
            int n = System.Math.Min(prevPath.Count, currPath.Length); 
            for (int  i = 0 ; i < n ; i++) {
                if (currPath[i] != prevPath.Array[prevPath.Offset + i]) {
                    break;
                }
                keep++;
            }

            Debug.Log("Routing.leave");
            var prevPlan = new Plan(prevPath, keep);
            self.concreteRouter.leave.OnNext(prevPlan);
            Debug.Log("Routing.leave done");
        }

        Debug.Log("Routing.enter");
        var currPlan = new Plan(new ArraySegment<string>(currPath), keep);
        prevPath = currPlan.path;
        self.concreteRouter.enter.OnNext(currPlan);
    }

    public static void Mount(string path, Router router) {
        var currPath = path.Split('/');
        var mount = new Mount(new ArraySegment<string>(currPath), router);
        self.concreteRouter.mount.OnNext(mount);
    }

}
