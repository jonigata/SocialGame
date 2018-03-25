using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

public class MockRouter : Router {
    [SerializeField] string leadPath;

    void Start() {
        var lp = leadPath.Split('/');

        enter
            .Where(x => Match(x.path, lp))
            .Subscribe(
                plan => {
                    Debug.Log("MockRouter(Enter)");
                    ProceedEnter(plan, lp.Length);
                })
            .AddTo(this);

        leave
            .Subscribe(
                plan => {
                    Debug.Log("NodeRouter(Leave)");
                    ProceedLeave(plan, lp.Length);
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

