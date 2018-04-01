using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class LazySceneLoadRouter : Router {
    [SerializeField] TransitionQueue queue;
    public SceneObject scene;
    [SerializeField] LoadSceneMode mode;

    Router parentRouter;

    void Start() {
        parentRouter = transform.parent.GetComponent<Router>();
        if (parentRouter == null) {
            Debug.LogWarningFormat(
                "LazySceneLoadRouter '{0}' has no parent Router", gameObject.name);
            return;
        }

        parentRouter.mount
            .Where(mount => mount.MatchHead(scene.name))
            .Subscribe(
                mount => {
                    Debug.Log("LazySceneLoadRouter.mount");
                    mount.router.MountTo(this);
                    ProceedMount(mount, 1);
                })
            .AddTo(this);

        parentRouter.leave
            .Where(plan => plan.MatchHead(scene.name))
            .Subscribe(
                plan => {
                    Debug.Log("LazySceneLoadRouter.leave");
                    if (plan.keep <= 0) {
                        queue.Post(
                            TransitionQueue.Request.Type.Out,
                            () => { SceneManager.UnloadScene(scene); },
                            () => { ProceedLeave(plan, 1); });
                    } else {
                        ProceedLeave(plan, 1);
                    }
                })
            .AddTo(this);

        parentRouter.enter
            .Where(plan => plan.MatchHead(scene.name))
            .Subscribe(
                plan => {
                    Debug.Log("LazySceneLoadRouter.enter");
                    ProceedEnter(plan, 1);
                    if (plan.keep <= 0) {
                        queue.Post(
                            TransitionQueue.Request.Type.In,
                            () => { SceneManager.LoadScene(scene, mode); },
                            () => {});
                    }
                })
            .AddTo(this);

    }

}
