using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class SceneLoader : MonoBehaviour {
    [SerializeField] SceneObject scene;

    void Awake() {
        GetComponent<Button>().onClick.AddListener(
            () => {
                SceneManager.LoadScene(scene);
            });
    }
	
}
