using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jump : MonoBehaviour {
    public void JumpTo(string s) {
        TopLevelRouter.MoveTo(s);
    }
}
