using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour {
    [SerializeField] PageGroup group;

    void OnEnable() {
        group.Select(transform);
    }
}
