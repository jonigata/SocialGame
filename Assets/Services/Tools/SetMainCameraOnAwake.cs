using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMainCameraOnAwake : MonoBehaviour {
    void Awake() {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
}
