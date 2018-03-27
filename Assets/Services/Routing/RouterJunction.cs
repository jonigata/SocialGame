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
        Debug.Log("RouterJunction.Start");
        TopLevelRouter.Mount(mountTo, this);
    }

    public override void MountTo(Router parentRouter) {
        Debug.Log("RouterJunction.MountTo");

        // translucent
        parentRouter.mount
            .Subscribe(
                mount => {
                    this.mount.OnNext(mount);
                })
            .AddTo(this);
        
        parentRouter.enter
            .Subscribe(
                plan => {
                    plan.Print();
                    this.enter.OnNext(plan);
                })
            .AddTo(this);

        parentRouter.leave
            .Subscribe(
                plan => {
                    this.leave.OnNext(plan);
                })
            .AddTo(this);
    }


}
