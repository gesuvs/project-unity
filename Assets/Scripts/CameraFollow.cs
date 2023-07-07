using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset;
    private Vector3 _velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        var isTargetNull = target.IsUnityNull();
        if (isTargetNull) return;
        var targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }
}
