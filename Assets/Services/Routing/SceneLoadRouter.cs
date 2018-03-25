using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class SceneLoadRouter : Router {
    [SerializeField] SceneLoadQueue queue;
    public SceneObject scene;
    [SerializeField] LoadSceneMode mode;
    [SerializeField] Router parentRouter;

    void Start() {
        parentRouter.leave
            .Where(plan => plan.MatchHead(scene.name))
            .Subscribe(
                plan => {
                    if (plan.keep <= 0) {
                        queue.Post(
                            SceneLoadQueue.Request.Type.Unload, scene, mode,
                            () => { ProceedLeave(plan); });
                    } else {
                        ProceedLeave(plan);
                    }
                })
            .AddTo(this);

        parentRouter.enter
            .Where(plan => plan.MatchHead(scene.name))
            .Subscribe(
                plan => {
                    ProceedEnter(plan);
                    if (plan.keep <= 0) {
                        queue.Post(
                            SceneLoadQueue.Request.Type.Load, scene, mode,
                            () => {});
                    }
                })
            .AddTo(this);

    }

    public override void SetParentRouter(Router r) { parentRouter = r; }
}
