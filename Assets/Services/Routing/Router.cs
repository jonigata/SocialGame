using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Router : MonoBehaviour {
    public class Plan {
        public ArraySegment<string> path;
        public int                  keep;

        public Plan(ArraySegment<string> path, int keep) {
            this.path = path;
            this.keep = keep;
        }
        public bool MatchHead(string s) {
            if (path.Array.Length <= path.Offset ) {
                return false;
            }
            return path.Array[path.Offset] == s;
        }
        public void Print() {
            string s = "[";
            for (int i = 0 ; i < path.Array.Length ; i++) {
                if (i == path.Offset) { s += "<color=yellow>"; }
                if (i == path.Offset + path.Count) { s += "</color>"; }
                s += path.Array[i];
                if (i != path.Array.Length - 1) { s += ","; }
            }
            s += "]";
            Debug.Log(s);
        }
    }

    public class Mount {
        public ArraySegment<string>    path;
        public Router                  router;

        public Mount(ArraySegment<string> path, Router router) {
            this.path = path;
            this.router = router;
        }
        public bool MatchHead(string s) {
            if (path.Array.Length <= path.Offset ) {
                return false;
            }
            return path.Array[path.Offset] == s;
        }
    }

    [NonSerialized] public BehaviorSubject<Plan> enter =
        new BehaviorSubject<Plan>(null);
    [NonSerialized] public BehaviorSubject<Plan> leave =
        new BehaviorSubject<Plan>(null);
    [NonSerialized] public BehaviorSubject<Mount> mount =
        new BehaviorSubject<Mount>(null);

    protected void Proceed(BehaviorSubject<Plan> subject, Plan plan, int n) {
        if (plan.path.Count < n) { return; }

        var newPlan = new Plan(
            new ArraySegment<string>(
                plan.path.Array, plan.path.Offset + n, plan.path.Count - n),
            plan.keep - n);
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

        var newMount = new Mount(
            new ArraySegment<string>(
                mount.path.Array, mount.path.Offset + n, mount.path.Count - n),
            mount.router);
        this.mount.OnNext(newMount);
    }

    public virtual void MountTo(Router router) {}

}
