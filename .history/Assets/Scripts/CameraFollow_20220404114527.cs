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
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }
        var newPosition = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);

        newPosition.x = Mathf.Clamp(newPosition.x, minPosition, maxPosition);
        newPosition.y = transform.position.y;
        newPosition.z = transform.position.z;

        transform.position = newPosition;
    }
}
