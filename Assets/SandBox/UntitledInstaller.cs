using UnityEngine;
using Zenject;

public class UntitledInstaller : MonoInstaller<UntitledInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ClassA>().AsSingle();
    }
}