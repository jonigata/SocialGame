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

    protected void Proceed(BehaviorSubject<Plan> subject, Plan plan, int n) {
        if (plan.path.Count < n) { return; }

        var newPlan = new Plan();
        newPlan.path = new ArraySegment<string>(
            plan.path.Array, plan.path.Offset + n, plan.path.Count - n);
        newPlan.keep = plan.keep - n;
        subject.OnNext(newPlan);
    }

    protected void ProceedEnter(Plan plan, int n) {
        Proceed(enter, plan, n);
    }

    protected void ProceedLeave(Plan plan, int n) {
        Proceed(leave, plan, n);
    }

}
