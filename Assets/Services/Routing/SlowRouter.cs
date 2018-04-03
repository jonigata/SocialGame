using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class SlowRouter : Router {
    [SerializeField] TransitionQueue queue;
    [SerializeField] RoutingTrigger trigger;

    Router parentRouter;

    void Start() {
        parentRouter = transform.parent.GetComponent<Router>();
        if (parentRouter == null) {
            Debug.LogWarningFormat(
                "TransitioningRouter '{0}' has no parent Router",
                gameObject.name);
            return;
        }

        var nodeName = gameObject.name;

        parentRouter.mount
            .Where(mount => mount != null && mount.MatchHead(nodeName))
            .Subscribe(
                mount => {
                    if (mount.path.Count == 1) {
                        Debug.LogFormat(
                            "<color=green>LazySceneLoadRouter.mount: {0}({1}) to {2}({3})</color>",
                            mount.router.gameObject.name,
                            mount.router.gameObject.scene.name,
                            this.gameObject.name,
                            this.gameObject.scene.name);
                        mount.Print();
                        mount.router.MountTo(this);
                    }
                    ProceedMount(mount, 1);
                })
            .AddTo(this);

        parentRouter.leave
            .Where(plan => plan != null && plan.MatchHead(nodeName))
            .Subscribe(
                plan => {
                    Debug.Log("LazySceneLoadRouter.leave");
                    if (plan.keep <= 0) {
                        queue.Post(
                            TransitionQueue.Request.Type.Out,
                            () => {
                            },
                            () => {
                                ProceedLeave(plan, 1);
                                if (trigger != null) { trigger.OnLeave(); }
                            });
                    } else {
                        ProceedLeave(plan, 1);
                    }
                })
            .AddTo(this);

        parentRouter.enter
            .Where(plan => plan != null && plan.MatchHead(nodeName))
            .Subscribe(
                plan => {
                    Debug.Log("LazySceneLoadRouter.enter");
                    if (plan.keep <= 0) {
                        queue.Post(
                            TransitionQueue.Request.Type.In,
                            () => {
                                if (trigger != null) { trigger.OnEnter(); }
                                ProceedEnter(plan, 1);
                            },
                            () => {
                            });
                    } else {
                        ProceedEnter(plan, 1);
                    }
                })
            .AddTo(this);

    }

}
