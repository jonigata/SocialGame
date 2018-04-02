using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour {
    [SerializeField] SceneObject standardHeader;
    [SerializeField] SceneObject standardFooter;

    [SerializeField] SceneObject storyPage;
    [SerializeField] SceneObject coordinatePage;
    [SerializeField] SceneObject trainingPage;
    [SerializeField] SceneObject socialPage;
    [SerializeField] SceneObject shopPage;

    [SerializeField] PageGroup pageGroup;

    void Awake() {
        SceneManager.LoadScene(standardHeader, LoadSceneMode.Additive);
        SceneManager.LoadScene(standardFooter, LoadSceneMode.Additive);

        SceneManager.LoadScene(storyPage, LoadSceneMode.Additive);
        SceneManager.LoadScene(coordinatePage, LoadSceneMode.Additive);
        SceneManager.LoadScene(trainingPage, LoadSceneMode.Additive);
        SceneManager.LoadScene(socialPage, LoadSceneMode.Additive);
        SceneManager.LoadScene(shopPage, LoadSceneMode.Additive);
    }

    void Start() {
        pageGroup.SetPages(FindObjectsOfType<HomePage>());
        Services.FullScreenFadeIn();
    }
}
