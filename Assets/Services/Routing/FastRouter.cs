using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UniRx;

public class FastRouter : Router {
    [SerializeField] protected RoutingTrigger trigger;
    
    Router parentRouter;

    void Start() {
        Debug.LogFormat("NodeRouter({0}).Start", gameObject.name);
        if (transform.parent == null) { return; }
        parentRouter = transform.parent.GetComponent<Router>();
        if (parentRouter == null) { return; }

        MountTo(parentRouter);
    }

    public override void MountTo(Router parentRouter) {
        Debug.LogFormat(
            "NodeRouter.Mount {0}({1}) To {2}({3})",
            this.gameObject.name,
            this.gameObject.scene.name,
            parentRouter.gameObject.name,
            parentRouter.gameObject.scene.name);

        var nodeName = gameObject.name;

        parentRouter.mount
            .Where(mount => mount != null && mount.MatchHead(nodeName))
            .Subscribe(
                mount => {
                    if (mount.path.Count == 1) {
                        Debug.LogFormat(
                            "<color=green>NodeRouter.mount: {0}({1}) to {2}({3})</color>",
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
                    Debug.Log("NodeRouter(Leave): " + nodeName);
                    ProceedLeave(plan, 1);
                    if (trigger != null) {
                        trigger.OnLeave();
                    }
                })
            .AddTo(this);

        parentRouter.enter
            .Where(plan => plan != null && plan.MatchHead(nodeName))
            .Subscribe(
                plan => {
                    Debug.Log("NodeRouter(Enter): " + nodeName);
                    if (trigger != null) {
                        trigger.OnEnter();
                    }
                    ProceedEnter(plan, 1);
                })
            .AddTo(this);

    }

}
