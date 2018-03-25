using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageGroup : MonoBehaviour {
    [SerializeField] Transform[] pages;
    [SerializeField] Transform initialPage;

    void Awake() {
        Select(initialPage);
    }

    public void Select(Transform page) {
        foreach (var e in pages) {
            var f = e == page;
            if (f != e.gameObject.activeSelf) {
                e.gameObject.SetActive(f);
            }
        }
    }

    public IEnumerator SelectAsync(Transform page) {
        yield return null; // fade out
        Select(page);
        yield return null; // fade in
    }

    public void SelectAsyncWithCallback(Transform page, Action a) {
        StartCoroutine(SelectAsyncWithCallbackAux(page, a));
    }

    public IEnumerator SelectAsyncWithCallbackAux(Transform page, Action a) {
        yield return SelectAsync(page);
        a();
    }

    
}
