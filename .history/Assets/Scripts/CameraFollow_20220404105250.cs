

using UnityEngine;
using System.Collections;

namespace Yarn.Unity.Example
{

    /// Control the position of the camera and its behaviour
    /** Camera should have minPosition and maxPosition of the
     * same because we're dealing with 2D. The movement speed
     * shouldn't be too fast nor too slow
     */
    public class CameraFollow : MonoBehaviour
    {

        /// Target of the camera
        public Transform target;

        /// Minimum position of camera
        public float minPosition = -5.3f;

        /// Maximum position of camera
        public float maxPosition = 5.3f;

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
}

