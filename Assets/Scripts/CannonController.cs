using UnityEngine;

public class CannonController : MonoBehaviour 
{
    [SerializeField]
    Transform cannonBase;

    [SerializeField]
    Transform turret;

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    Transform smokePuffPoint;

    [SerializeField]
    Animation anim;

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    GameObject cannonFirePrefab;

    [SerializeField]
    ProjectileArc projectileArc;

    [SerializeField]
    float cooldown = 1;

    private float currentSpeed;
    private float currentAngle;
    private float currentTimeOfFlight;

    public float lastShotTime { get; private set; }
    public float lastShotTimeOfFlight { get; private set; }

    public void SetTargetWithAngle(Vector3 point, float angle)
    {
        currentAngle = angle;

        Vector3 direction = point - firePoint.position;
        float yOffset = -direction.y;
        direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;

        currentSpeed = ProjectileMath.LaunchSpeed(distance, yOffset, Physics.gravity.magnitude, angle * Mathf.Deg2Rad);

        projectileArc.UpdateArc(currentSpeed, distance, Physics.gravity.magnitude, currentAngle * Mathf.Deg2Rad, direction, true);
        SetTurret(direction, currentAngle);

        currentTimeOfFlight = ProjectileMath.TimeOfFlight(currentSpeed, currentAngle * Mathf.Deg2Rad, yOffset, Physics.gravity.magnitude);
    }

    public void SetTargetWithSpeed(Vector3 point, float speed, bool useLowAngle)
    {
        currentSpeed = speed;

        Vector3 direction = point - firePoint.position;
        float yOffset = direction.y;
        direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;     

        float angle0, angle1;
        bool targetInRange = ProjectileMath.LaunchAngle(speed, distance, yOffset, Physics.gravity.magnitude, out angle0, out angle1);

        if (targetInRange)
            currentAngle = useLowAngle ? angle1 : angle0;                     

        projectileArc.UpdateArc(speed, distance, Physics.gravity.magnitude, currentAngle, direction, targetInRange);
        SetTurret(direction, currentAngle * Mathf.Rad2Deg);

        currentTimeOfFlight = ProjectileMath.TimeOfFlight(currentSpeed, currentAngle, -yOffset, Physics.gravity.magnitude);
    }

    public void Fire()
    {
        if (Time.time > lastShotTime + cooldown)
        {
            GameObject p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            p.GetComponent<Rigidbody>().velocity = turret.up * currentSpeed;

            Instantiate(cannonFirePrefab, smokePuffPoint.position, Quaternion.LookRotation(turret.up));

            lastShotTime = Time.time;
            lastShotTimeOfFlight = currentTimeOfFlight;

            anim.Rewind();
            anim.Play();
        }
    }

    private void SetTurret(Vector3 planarDirection, float turretAngle)
    {
        cannonBase.rotation =  Quaternion.LookRotation(planarDirection) * Quaternion.Euler(-90, -90, 0);
        turret.localRotation = Quaternion.Euler(90, 90, 0) * Quaternion.AngleAxis(turretAngle, Vector3.forward);        
    }
}
