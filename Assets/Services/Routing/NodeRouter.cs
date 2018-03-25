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
    [Serializable]
    public struct Triggers {
        public UnityEvent onEnter;
        public UnityEvent onLeave;
    }
    
    [SerializeField] Triggers triggers;
    [SerializeField] GameObject activates;
    
    Router parentRouter;

    void Start() {
        if (transform.parent == null) { return; }
        parentRouter = transform.parent.GetComponent<Router>();
        if (parentRouter == null) { return; }

        var nodeName = gameObject.name;

        parentRouter.enter
            .Where(plan => plan.MatchHead(nodeName))
            .Subscribe(
                plan => {
                    Debug.Log("NodeRouter(Enter): " + nodeName);
                    triggers.onEnter.Invoke();
                    if (activates != null) { activates.SetActive(true); }
                    ProceedEnter(plan, 1);
                })
            .AddTo(this);

        parentRouter.leave
            .Where(plan => plan.MatchHead(nodeName))
            .Subscribe(
                plan => {
                    Debug.Log("NodeRouter(Leave): " + nodeName);
                    ProceedLeave(plan, 1);
                    if (activates != null) { activates.SetActive(false); }
                    triggers.onLeave.Invoke();
                })
            .AddTo(this);
    }

}
