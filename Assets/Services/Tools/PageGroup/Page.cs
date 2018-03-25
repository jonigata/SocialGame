using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour {
    [SerializeField] PageGroup group;

    void OnEnable() {
        if (group != null) {
            group.Select(transform);
        }
    }

    public void SetPageGroup(PageGroup group) {
        this.group = group;
    }
}
