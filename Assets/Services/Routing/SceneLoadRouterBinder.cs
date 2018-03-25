using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class SceneLoadRouterBinder : Router {
    [SerializeField] Router fallbackRouter;
    Router parentRouter;
    
    void Awake() {
        var a = FindObjectsOfType<SceneLoadRouter>();
        parentRouter =
            a.FirstOrDefault(x => x.scene.name == gameObject.scene.name);
        if (parentRouter == null) {
            parentRouter = fallbackRouter;
        }
        if (parentRouter == null) { return; }

        // translucent
        
        parentRouter.enter
            .Subscribe(
                plan => {
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
