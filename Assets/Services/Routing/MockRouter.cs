using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

public class MockRouter : Router {
    [SerializeField] Router parentRouter;
    [SerializeField] string leadPath;

    void Start() {
        var lp = leadPath.Split('/');

        parentRouter.mount
            .Where(mount => mount != null && Match(mount.path, lp))
            .Subscribe(
                mount => {
                    Debug.Log("MockRouter(Mount)");
                    if (mount.path.Count == lp.Length) {
                        mount.router.MountTo(this);
                    }
                    ProceedMount(mount, lp.Length);
                })
            .AddTo(this);

        parentRouter.leave
            .Where(plan => plan != null && Match(plan.path, lp))
            .Subscribe(
                plan => {
                    Debug.Log("MockRouter(Leave)");
                    ProceedLeave(plan, lp.Length);
                })
            .AddTo(this);

        parentRouter.enter
            .Where(plan => plan != null && Match(plan.path, lp))
            .Subscribe(
                plan => {
                    Debug.Log("MockRouter(Enter)");
                    ProceedEnter(plan, lp.Length);
                })
            .AddTo(this);
    }

    bool Match(ArraySegment<string> x, string[] y) {
        if (x.Count < y.Length) { return false; }
        for (int i = 0 ; i < y.Length ; i++) {
            if (x.Array[x.Offset+i] != y[i]) { return false; }
        }
        return true;
    }
    

}

