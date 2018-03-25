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
    [SerializeField] Router parentRouter;
    [SerializeField] string nodeName;

    [SerializeField] UnityEvent onEnter;
    [SerializeField] UnityEvent onLeave;
    [SerializeField] GameObject activates;
    
    void Start() {
        if (parentRouter == null) { return; }

        parentRouter.enter
            .Where(plan => plan.MatchHead(nodeName))
            .Subscribe(
                plan => {
                    Debug.Log("NodeRouter(Enter): " + nodeName);
                    onEnter.Invoke();
                    if (activates != null) { activates.SetActive(true); }
                    ProceedEnter(plan);
                })
            .AddTo(this);

        parentRouter.leave
            .Where(plan => plan.MatchHead(nodeName))
            .Subscribe(
                plan => {
                    Debug.Log("NodeRouter(Leave): " + nodeName);
                    ProceedLeave(plan);
                    if (activates != null) { activates.SetActive(false); }
                    onLeave.Invoke();
                })
            .AddTo(this);
    }

    public override void SetParentRouter(Router r) { parentRouter = r; }

}
