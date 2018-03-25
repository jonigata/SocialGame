using System;
using UnityEngine;

public class FullScreenFader : Router {
    void OnEnable() {
        Services.FullScreenFadeIn();
    }

    void OnDisable() {
        Services.FullScreenFadeOut();
    }
}
