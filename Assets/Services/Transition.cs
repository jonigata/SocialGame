using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Transition : MonoBehaviour {
    public abstract IEnumerator In();
    public abstract IEnumerator Out();
}
