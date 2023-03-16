using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Shootable : MonoBehaviour, IShootable
{
    public int GeneralLayer;
    public GameObject Owner { get; set; }
    public int damage { get; set; } = 33;
    public float Gravity = 9.82f;
    public float speed = 5;
    public Vector3 velocity = Vector3.zero;
    protected bool shot = false;

    public bool fall = false;

    protected Vector3 hitPoint;
    protected GameObject hitObject;

    public float Charge = 0;
    public float Speed { get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    public float maxDistance = 100;
    public float MaxDistance
    {
        get
        {
            return maxDistance;
        }
        set
        {
            maxDistance = value;
        }
    }

    private float travelDistance = 0f;
    public float correctionDistance = 5f;
    private Vector3 correction;
    private Vector3 prevPos;
    // Start is called before the first frame update

    public virtual void Shoot(WeaponController controller)
    {
        Owner = controller.Owner;
        velocity = transform.forward * Speed;
        prevPos = transform.position;
        Charge = controller.Charge;
        if (correctionDistance >= 0)
        {
            PlayerWeaponManager pwm = this.Owner.GetComponent<PlayerWeaponManager>();
            correction = pwm.cam.transform.position - transform.position;
            correction = Vector3.ProjectOnPlane(correction, pwm.cam.transform.forward);
        }
        shot = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (shot || fall) {
            Fly();
            if (correction.magnitude > 0.0001)
            {
                Vector3 corrToApply = (prevPos - transform.position).magnitude / correctionDistance * correction;
                corrToApply = Vector3.ClampMagnitude(corrToApply, correction.magnitude);
                correction -= corrToApply;
                transform.position += corrToApply;
            }
            HitScan();
            prevPos = transform.position;
        }
    }

    public virtual void Fly()
    {
        transform.position += velocity * Time.deltaTime;
        travelDistance += Speed * Time.deltaTime;
        velocity += Vector3.down * Gravity * Time.deltaTime;
        transform.forward = velocity.normalized;
        if(travelDistance > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public float Radius = .01f;
    public LayerMask hitMask;
    public void HitScan()
    {
        RaycastHit closestHit = new RaycastHit();
        Vector3 displacement = transform.position - prevPos;
        if(Physics.SphereCast(prevPos, Radius, displacement.normalized, out closestHit, displacement.magnitude, hitMask))
        {

            hitPoint = closestHit.point;
            hitObject = closestHit.collider.gameObject;
            Hit(closestHit);
        }
    }

    public void Hit(RaycastHit hit)
    {
        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
        if(damageable != null) {
            damageable.Hit(this.damage);
        }

        HitEffects(); 
    }

    public virtual void HitEffects()
    {
        Destroy(gameObject);
    }
}
