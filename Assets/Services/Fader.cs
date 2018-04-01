using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fader : Transition {
    [SerializeField] GameObject fadeIn;
    [SerializeField] GameObject fadeOut;

    void Awake() {
        fadeIn.SetActive(false);
        fadeOut.SetActive(false);
    }

    IEnumerator PlayFade(GameObject fade) {
        fade.SetActive(true);
        var a = fade.GetComponentInChildren<Animator>();
        yield return new WaitWhile(
            () => a.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
    }

    public override IEnumerator In() {
        fadeOut.SetActive(false);
        yield return PlayFade(fadeIn);
    }

    public override IEnumerator Out() {
        fadeIn.SetActive(false);
        yield return PlayFade(fadeOut);
    }

}
