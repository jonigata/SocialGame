using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class NodeRouter : Router {
    // TODO: TriggerableRouterを作る
    [Serializable]
    public struct Triggers {
        public UnityEvent onEnter;
        public UnityEvent onLeave;
        public GameObject activateTarget;


        public void OnEnter() {
            onEnter.Invoke();
            if (activateTarget != null) { activateTarget.SetActive(true); }
        }
        public void OnLeave() {
            if (activateTarget != null) { activateTarget.SetActive(false); }
            onLeave.Invoke();
        }
    }
    
    [SerializeField] protected Triggers triggers;
    
    Router parentRouter;

    void Start() {
        Debug.Log("NodeRouter.Start");
        if (transform.parent == null) { return; }
        parentRouter = transform.parent.GetComponent<Router>();
        if (parentRouter == null) { return; }

        MountTo(parentRouter);
    }

    public override void MountTo(Router parentRouter) {
        var nodeName = gameObject.name;

        parentRouter.mount
            .Where(mount => mount.MatchHead(nodeName))
            .Subscribe(
                mount => {
                    Debug.Log("NodeRouter(Mount)");
                    if (mount.path.Count == 1) {
                        mount.router.MountTo(this);
                    }
                    ProceedMount(mount, 1);
                })
            .AddTo(this);

        parentRouter.leave
            .Where(plan => plan.MatchHead(nodeName))
            .Subscribe(
                plan => {
                    Debug.Log("NodeRouter(Leave): " + nodeName);
                    ProceedLeave(plan, 1);
                    triggers.OnLeave();
                })
            .AddTo(this);

        parentRouter.enter
            .Where(plan => plan.MatchHead(nodeName))
            .Subscribe(
                plan => {
                    Debug.Log("NodeRouter(Enter): " + nodeName);
                    triggers.OnEnter();
                    ProceedEnter(plan, 1);
                })
            .AddTo(this);

    }

}
