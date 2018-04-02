using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class RouterJunction : FastRouter {
    [SerializeField] string mountTo;
    
    void Start() {
        // base.Start(); 必要ない
        Debug.LogFormat("RouterJunction.Start {0}({1})", gameObject.name, gameObject.scene.name);
        Routing.Mount(mountTo, this);
    }

    public override void MountTo(Router parentRouter) {
        Debug.LogFormat(
            "RouterJunction.Mount {0}({1}) To {2}({3})",
            this.gameObject.name,
            this.gameObject.scene.name,
            parentRouter.gameObject.name,
            parentRouter.gameObject.scene.name);

        // translucent
        parentRouter.mount
            .Subscribe(
                mount => {
                    this.mount.OnNext(mount);
                })
            .AddTo(this);

        parentRouter.leave
            .Where(plan => plan != null)
            .Subscribe(
                plan => {
                    Debug.LogFormat(
                        "RouterJunction.leave: {0}({1})",
                        gameObject.name,
                        gameObject.scene.name);
                    plan.Print();
                    this.leave.OnNext(plan);
                    Debug.Log("RouterJunction.leave: triggers done");
                    if (trigger != null) {
                        trigger.OnLeave();
                    }
                })
            .AddTo(this);
        
        parentRouter.enter
            .Where(plan => plan != null)
            .Subscribe(
                plan => {
                    Debug.LogFormat(
                        "RouterJunction.enter: {0}({1})",
                        gameObject.name,
                        gameObject.scene.name);
                    plan.Print();
                    if (trigger != null) {
                        trigger.OnEnter();
                    }
                    Debug.Log("RouterJunction.enter: triggers done");
                    this.enter.OnNext(plan);
                })
            .AddTo(this);
    }


}
