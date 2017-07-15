using UnityEngine;

public class SkyCamera : MonoBehaviour 
{
    [SerializeField]
    float incline = 45;

    [SerializeField]
    float minDistance = 10;

    [SerializeField]
    float maxDistance = 50;

    [SerializeField]
    float speed = 10;

    [SerializeField]
    float acceleration = 50;

    [SerializeField]
    float decceleration = 70;

    [SerializeField]
    float zoomSensitivity = 1;

    private Vector3 velocity;
    private Vector3 position;

    private float currentDistance;

    void Awake()
    {
        velocity = Vector3.zero;

        currentDistance = (minDistance + maxDistance) * 0.5f;

        transform.rotation = Quaternion.AngleAxis(incline, Vector3.right);
    }

	void Update () 
	{
        float zoomInput = -Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;

        currentDistance = Mathf.Clamp(currentDistance + zoomInput, minDistance, maxDistance);

        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (moveInput != Vector3.zero)
            velocity = Vector3.MoveTowards(velocity, moveInput.normalized * speed, acceleration * Time.deltaTime);
        else
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, decceleration * Time.deltaTime);

        position += velocity;

        transform.position = -transform.forward * currentDistance + position;
    }
}
