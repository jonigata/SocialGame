using UnityEngine;
using UnityEngine.Assertions;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    public static T Self { get { return (T)self; } }
    protected static Singleton<T> self;

    protected void Awake() {
        Assert.IsNull(self);
        self = this;
    }

    protected void OnDestroy() {
        Assert.IsTrue(self == this);
        self = null;
    }

}
