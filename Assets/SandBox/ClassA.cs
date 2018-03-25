using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ClassA : MonoBehaviour {
    [Inject]
    private ClassB _classB;

    void Start() {
        _classB.CreateLog();
    }

}
