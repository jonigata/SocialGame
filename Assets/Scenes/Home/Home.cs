using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour {
    [SerializeField] SceneObject standardHeader;
    [SerializeField] SceneObject standardFooter;

    void Awake() {
        SceneManager.LoadScene(standardHeader, LoadSceneMode.Additive);
        SceneManager.LoadScene(standardFooter, LoadSceneMode.Additive);
    }

    void Start() {
        Services.FullScreenFadeIn();
    }
}
