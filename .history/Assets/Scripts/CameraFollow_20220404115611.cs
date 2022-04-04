using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    /// Target of the camera
    public Transform target;

    /// Movement speed of camera
    public float moveSpeed = 1.0f;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
        var newPosition = Vector3.Lerp(transform.position, new Vector2(target.position.x,target.position.), moveSpeed * Time.deltaTime);

        transform.position = newPosition;
    }
}
