using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class SceneLoadRoutingTrigger : RoutingTrigger {
    public SceneObject scene;
    [SerializeField] LoadSceneMode mode;

    public override void OnEnter() {
        SceneManager.LoadScene(scene, mode);
    }

    public override void OnLeave() {
        SceneManager.UnloadScene(scene); 
    }

}
