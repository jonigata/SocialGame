using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Router : MonoBehaviour {
    public struct Plan {
        public ArraySegment<string>    path;
        public int                     keep;
        public bool MatchHead(string s) {
            if (path.Array == null || path.Array.Length <= path.Offset ) {
                return false;
            }
            return path.Array[path.Offset] == s;
        }
    }

    [NonSerialized] public BehaviorSubject<Plan> enter =
        new BehaviorSubject<Plan>(new Plan());
    [NonSerialized] public BehaviorSubject<Plan> leave =
        new BehaviorSubject<Plan>(new Plan());

    public virtual void SetParentRouter(Router r) {}
    
    protected void Proceed(BehaviorSubject<Plan> subject, Plan plan) {
        if (plan.path.Count == 0) { return; }

        var newPlan = new Plan();
        newPlan.path = new ArraySegment<string>(
            plan.path.Array, plan.path.Offset + 1, plan.path.Count - 1);
        newPlan.keep = plan.keep -1;
        subject.OnNext(newPlan);
    }

    protected void ProceedEnter(Plan plan) {
        Proceed(enter, plan);
    }

    protected void ProceedLeave(Plan plan) {
        Proceed(leave, plan);
    }
}
