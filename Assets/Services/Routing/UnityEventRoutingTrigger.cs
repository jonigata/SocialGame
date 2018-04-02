using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UniRx;

public class UnityEventRoutingTrigger : RoutingTrigger {
    public UnityEvent onEnter;
    public UnityEvent onLeave;

    public override void OnEnter() {
        onEnter.Invoke();
    }
    public override void OnLeave() {
        onLeave.Invoke();
    }
}
