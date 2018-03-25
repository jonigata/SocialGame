using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Services : Singleton<Services> {
    [SerializeField] Fader fader;

    void Awake() {
        base.Awake();
    }

    public static IEnumerator FullScreenFadeIn() {
        if (Self == null) { yield break; }
        yield return Self.fader.In();
    }

    public static IEnumerator FullScreenFadeOut() {
        if (Self == null) { yield break; }
        yield return Self.fader.Out();
    }

}
