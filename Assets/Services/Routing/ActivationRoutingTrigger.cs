using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UniRx;

public class ActivationRoutingTrigger : RoutingTrigger {
    public GameObject activateTarget;

    public override void OnEnter() {
        if (activateTarget != null) {
            Debug.Log("activating: " + activateTarget.name);
            activateTarget.SetActive(true);
        } else {
            Debug.Log("activating none");
        }
    }
    public override void OnLeave() {
        if (activateTarget != null) {
            Debug.Log("deactivating: " + activateTarget.name);
            activateTarget.SetActive(false);
        } else {
            Debug.Log("deactivating none");
        }
    }
}
