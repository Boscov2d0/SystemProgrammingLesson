using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;

public class Fractal : MonoBehaviour
{
    [SerializeField, Range(1, 8)] private int _depth = 4;
    [SerializeField, Range(1, 360)] private int _rotationSpeed;
    private const float _positionOffset = .75f;
    private const float _scaleBias = .5f;

    private TransformAccessArray _transformAccessArray;
    private Transform[] _transforms;

    private void Start()
    {
        name = "Fractal " + _depth;
        if (_depth <= 1)
        {
            return;
        }

        _transforms = new Transform[5];

        var childA = CreateChild(Vector3.up, Quaternion.identity);
        var childB = CreateChild(Vector3.right, Quaternion.Euler(0f, 0f, -90f));
        var childC = CreateChild(Vector3.left, Quaternion.Euler(0f, 0f, 90f));
        var childD = CreateChild(Vector3.forward, Quaternion.Euler(90f, 0f, 0f));
        var childE = CreateChild(Vector3.back, Quaternion.Euler(-90f, 0f, 0f));

        childA.transform.SetParent(transform, false);
        childB.transform.SetParent(transform, false);
        childC.transform.SetParent(transform, false);
        childD.transform.SetParent(transform, false);
        childE.transform.SetParent(transform, false);

        _transforms[0] = childA.transform;
        _transforms[1] = childB.transform;
        _transforms[2] = childC.transform;
        _transforms[3] = childD.transform;
        _transforms[4] = childE.transform;

        _transformAccessArray = new TransformAccessArray(_transforms);
    }
    private void Update()
    {
        if (_depth <= 1)
        {
            return;
        }
        RotateFractalJob jobForTransform = new RotateFractalJob()
        {
            Speed = _rotationSpeed,
            DeltaTime = Time.deltaTime
        };

        JobHandle jobHandle2 = jobForTransform.Schedule(_transformAccessArray);

        jobHandle2.Complete();
        
        //transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f);

    }
    private void OnDestroy()
    {
        if (_depth > 1)
        {
            _transformAccessArray.Dispose();
        }
    }
    private Fractal CreateChild(Vector3 direction, Quaternion rotation)
    {
        var child = Instantiate(this);
        child._depth = _depth - 1;
        child.transform.localPosition = _positionOffset * direction;
        child.transform.localRotation = rotation;
        child.transform.localScale = _scaleBias * Vector3.one;
        return child;
    }
}
