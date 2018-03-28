using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Router : MonoBehaviour {
    public struct Plan {
        public ArraySegment<string> path;
        public int                  keep;
        public bool MatchHead(string s) {
            if (path.Array == null || path.Array.Length <= path.Offset ) {
                return false;
            }
            return path.Array[path.Offset] == s;
        }
        public void Print() {
            Debug.Log(string.Join(",", path.ToArray()));
        }
    }

    public struct Mount {
        public ArraySegment<string>    path;
        public Router                  router;
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
    [NonSerialized] public BehaviorSubject<Mount> mount =
        new BehaviorSubject<Mount>(new Mount());

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

    protected void ProceedMount(Mount mount, int n) {
        if (mount.path.Count < n) { return; }

        var newMount = new Mount();
        newMount.path = new ArraySegment<string>(
            mount.path.Array, mount.path.Offset + n, mount.path.Count - n);
        newMount.router = mount.router;
        this.mount.OnNext(newMount);
    }

    public virtual void MountTo(Router router) {}

}
