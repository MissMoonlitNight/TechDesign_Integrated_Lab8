using UnityEngine;

public abstract class AbilityEffect : MonoBehaviour
{
    protected AbilityData data;
    protected Vector3 target;
    protected GameObject caster;

    public void Initialize(AbilityData abilityData, Vector3 targetPoint, GameObject source)
    {
        data = abilityData;
        target = targetPoint;
        caster = source;
        Execute();
    }

    protected abstract void Execute();
}