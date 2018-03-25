using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class SceneLoadRouterBinder : MonoBehaviour {
    [SerializeField] Router[] routers;
    
    void Awake() {
        var a = FindObjectsOfType<SceneLoadRouter>();
        var parentRouter =
            a.FirstOrDefault(x => x.scene.name == gameObject.scene.name);

        foreach (var router in routers) {
            router.SetParentRouter(parentRouter);
        }
    }

}
