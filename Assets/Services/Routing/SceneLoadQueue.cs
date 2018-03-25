using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class SceneLoadQueue : MonoBehaviour {
    [SerializeField] SceneObject intermission;
    [SerializeField] Fader fader;

    public struct Request {
        public enum Type {
            Load,
            Unload,
        }
        public Type        type;
        public SceneObject scene;
        public LoadSceneMode mode;
        public Action onDone;
    }

    Queue<Request> q = new Queue<Request>();
    bool intermissionActive;

    void Start() {
        StartCoroutine(Consume());
    }

    public void Post(
        Request.Type requestType, SceneObject scene, LoadSceneMode mode,
        Action onDone) {
        var r = new Request();
        r.type = requestType;
        r.scene = scene;
        r.mode = mode;
        r.onDone = onDone;
        q.Enqueue(r);
    }

    IEnumerator Consume() {
        while (true) {
            if (q.Count == 0) {         // こうしないと1フレ抜ける
                yield return new WaitUntil(() => 0 < q.Count);
            }
            var r = q.Dequeue();
            switch (r.type) {
                case Request.Type.Load:
                    SceneManager.LoadScene(r.scene, r.mode);
                    yield return null;
                    UnloadIntermission();
                    yield return fader.In();
                    break;
                case Request.Type.Unload:
                    yield return fader.Out();
                    LoadIntermission();
                    yield return null;
                    SceneManager.UnloadScene(r.scene);
                    break;
            }

            r.onDone();
        }
    }

    void LoadIntermission() {
        if (!intermissionActive) {
            SceneManager.LoadScene(intermission, LoadSceneMode.Additive);
            intermissionActive = true;
        }
    }

    void UnloadIntermission() {
        if (intermissionActive) {
            SceneManager.UnloadScene(intermission);
            intermissionActive = false;
        }
    }
}
