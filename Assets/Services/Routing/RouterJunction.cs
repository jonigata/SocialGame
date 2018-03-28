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
        Debug.Log("RouterJunction.MountTo: " + parentRouter.gameObject.name);

        // translucent
        parentRouter.mount
            .Subscribe(
                mount => {
                    this.mount.OnNext(mount);
                })
            .AddTo(this);

        parentRouter.leave
            .Subscribe(
                plan => {
                    Debug.Log("RouterJunction.leave: " + gameObject.name);
                    this.leave.OnNext(plan);
                    triggers.OnLeave();
                })
            .AddTo(this);
        
        parentRouter.enter
            .Subscribe(
                plan => {
                    Debug.Log("RouterJunction.enter: " + gameObject.name);
                    triggers.OnEnter();
                    this.enter.OnNext(plan);
                })
            .AddTo(this);
    }


}
