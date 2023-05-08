using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using System;

public class Lesson2 : MonoBehaviour, IDisposable
{
    private NativeArray<int> array;

    private NativeArray<Vector3> _positions;
    private NativeArray<Vector3> _velocities;
    private NativeArray<Vector3> _finalPositions;

    [SerializeField] private Transform _transformObject;
    [SerializeField] private Transform _transformObject2;
    private NativeArray<Vector3> _velocities2;
    private TransformAccessArray _transformAccessArray;

    private void Start()
    {
        array = new NativeArray<int>(15, Allocator.Persistent);

        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i;
            Debug.Log("array before: " + array[i]);
        }

        Job job = new Job();
        job.Array = array;

        JobHandle jobHandle = job.Schedule();
        jobHandle.Complete();

        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log("array after: " + array[i]);
        }

        array.Dispose();

        _positions = new NativeArray<Vector3>(5, Allocator.Persistent);
        _velocities = new NativeArray<Vector3>(5, Allocator.Persistent);

        for (int i = 0; i < _positions.Length; i++)
        {
            _positions[i] = new Vector3(i, i, i);
            _velocities[i] = new Vector3(i + 1, i + 1, i + 1);
        }

        _finalPositions = new NativeArray<Vector3>(5, Allocator.Persistent);

        JobParallelFor jobParallelFor = new JobParallelFor()
        {
            Positions = _positions,
            Velocities = _velocities,
            FinalPositions = _finalPositions
        };

        jobHandle = jobParallelFor.Schedule(_positions.Length, 0);
        jobHandle.Complete();

        for (int i = 0; i < _finalPositions.Length; i++)
        {
            Debug.Log("_finalPositions after: " + _finalPositions[i]);
        }

        _positions.Dispose();
        _velocities.Dispose();
        _finalPositions.Dispose();

        Transform[] transforms = new Transform[2];
        transforms[0] = _transformObject;
        transforms[1] = _transformObject2;
        _transformAccessArray = new TransformAccessArray(transforms);
        _velocities2 = new NativeArray<Vector3>(new Vector3[] { _transformObject.eulerAngles, _transformObject2.eulerAngles }, Allocator.Persistent);
    }
    private void Update()
    {
        JobForTransform jobForTransform = new JobForTransform()
        {
            Velocities = _velocities2,
            DeltaTime = Time.deltaTime
        };

        JobHandle jobHandle2 = jobForTransform.Schedule(_transformAccessArray);
        jobHandle2.Complete();
    }

    private void OnDestroy()
    {
        _velocities2.Dispose();
    }

    public void Dispose()
    {
        _velocities2.Dispose();
    }
}

public struct Job : IJob
{
    public NativeArray<int> Array;

    public void Execute()
    {
        for (int i = 0; i < Array.Length; i++)
        {
            if (Array[i] > 10)
            {
                Array[i] = 0;
            }
        }
    }
}

public struct JobParallelFor : IJobParallelFor
{
    public NativeArray<Vector3> Positions;
    public NativeArray<Vector3> Velocities;
    public NativeArray<Vector3> FinalPositions;

    public void Execute(int index)
    {
        FinalPositions[index] = Positions[index] + Velocities[index];
    }
}

public struct JobForTransform : IJobParallelForTransform
{
    public NativeArray<Vector3> Velocities;
    [ReadOnly] public float DeltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        Velocities[index] = Velocities[index] + new Vector3(1, 1, 1) * 0.01f * DeltaTime;
        transform.rotation *= Quaternion.Euler(Velocities[index]);
    }
}