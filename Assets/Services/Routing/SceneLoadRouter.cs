using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

public class SceneLoadRouter : NodeRouter {
    public SceneObject scene;
    [SerializeField] LoadSceneMode mode;

    void Awake() {
        SceneManager.LoadScene(scene, mode);
    }
}
