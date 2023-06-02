using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public struct RotateFractalJob : IJobParallelForTransform
{
    [ReadOnly] public float Speed;
    [ReadOnly] public float DeltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        transform.rotation *= Quaternion.Euler(new Vector3(0, Speed * DeltaTime, 0));
    }
}