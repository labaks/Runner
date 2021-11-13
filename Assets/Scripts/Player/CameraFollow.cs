using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    MovementController movementController;
    private Vector3 offset;
    Quaternion rotation;
    void Start()
    {
        rotation = transform.rotation;
        if (target != null)
        {
            offset = transform.position - target.position;
            movementController = target.GetComponent<MovementController>();
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, target.position.z) + offset, Time.deltaTime * 3);
            Quaternion targetRotation = movementController.targetRotation;
            targetRotation.x = rotation.x;
            transform.rotation = Quaternion.Slerp(rotation, targetRotation, 3f * Time.deltaTime);
        }
    }
}
