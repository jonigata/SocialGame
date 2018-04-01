using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class RouterJunction : NodeRouter {
    [SerializeField] string mountTo;
    
    void Start() {
        // base.Start(); 必要ない
        Debug.LogFormat("RouterJunction.Start {0}:{1}", gameObject.scene.name, gameObject.name);
        Routing.Mount(mountTo, this);
    }

    public override void MountTo(Router parentRouter) {
        Debug.LogFormat(
            "RouterJunction.Mount {0} To: {1}",
            this.gameObject.name,
            parentRouter.gameObject.name);

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
                    Debug.Log("RouterJunction.leave: " + gameObject.name);
                    plan.Print();
                    this.leave.OnNext(plan);
                    Debug.Log("RouterJunction.leave: triggers done");
                    triggers.OnLeave();
                })
            .AddTo(this);
        
        parentRouter.enter
            .Where(plan => plan != null)
            .Subscribe(
                plan => {
                    Debug.Log("RouterJunction.enter: " + gameObject.name);
                    plan.Print();
                    triggers.OnEnter();
                    Debug.Log("RouterJunction.enter: triggers done");
                    this.enter.OnNext(plan);
                })
            .AddTo(this);
    }


}
