using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class TransitionQueue : MonoBehaviour {
    [SerializeField] SceneObject intermission;
    [SerializeField] Transition transition;

    public struct Request {
        public enum Type {
            In,
            Out,
        }
        public Type             type;
        public Action           before;
        public Action           after;
 }

    Queue<Request> q = new Queue<Request>();
    bool intermissionActive;

    void Start() {
        StartCoroutine(Consume());
    }

    public void Post(Request.Type t, Action before, Action after) {
        var r = new Request();
        r.type = t;
        r.before = before;
        r.after  = after;
        q.Enqueue(r);
    }

    IEnumerator Consume() {
        while (true) {
            if (q.Count == 0) {         // こうしないと1フレ抜ける
                yield return new WaitUntil(() => 0 < q.Count);
            }
            var r = q.Dequeue();
            switch (r.type) {
                case Request.Type.In:
                    r.before();
                    yield return null;
                    UnloadIntermission();
                    if (transition != null) {
                        yield return transition.In();
                    }
                    r.after();
                    break;
                case Request.Type.Out:
                    r.before();
                    if (transition != null) {
                        yield return transition.Out();
                    }
                    LoadIntermission();
                    yield return null;
                    r.after();
                    break;
            }
        }
    }

    void LoadIntermission() {
        if (!intermissionActive) {
            if (!string.IsNullOrEmpty(intermission)) {
                SceneManager.LoadScene(intermission, LoadSceneMode.Additive);
            }
            intermissionActive = true;
        }
    }

    void UnloadIntermission() {
        if (intermissionActive) {
            if (!string.IsNullOrEmpty(intermission)) {
                SceneManager.UnloadScene(intermission);
            }
            intermissionActive = false;
        }
    }
}
