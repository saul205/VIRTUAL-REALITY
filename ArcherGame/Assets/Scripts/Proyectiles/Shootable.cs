using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Shootable : MonoBehaviour, IShootable
{
    public enum States
    {
        None,
        Flying,
        Hit,
        Fall,
        Grounded
    }

    public Shootable prefab;

    public States state = States.None;
    public Rigidbody Rigidbody;
    public GameObject Owner { get; set; }

    public GameObject impactImage;
    
    public int damage = 34;
    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    public float speed = 5;

    public float Charge = 0;
    public Transform tail;
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

    public float correctionDistance = 5f;
    private Vector3 correction;
    protected Vector3 prevPos;
    // Start is called before the first frame update

    public virtual void Shoot(WeaponController controller)
    {
        Owner = controller.Owner;
        prefab = controller.bulletPrefab;
        Rigidbody.AddForce(transform.forward * Speed, ForceMode.Impulse);
        prevPos = transform.position;
        Charge = controller.Charge;
        Rigidbody.useGravity = true;
        if (correctionDistance >= 0)
        {
            PlayerWeaponManager pwm = this.Owner.GetComponent<PlayerWeaponManager>();
            correction = pwm.aimSpot.transform.position - transform.position;
            correction = Vector3.ProjectOnPlane(correction, pwm.aimSpot.transform.forward);
        }
        state = States.Flying;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (state)
        {
            case States.Flying:
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
                break;
            case States.Hit:
                HitEffects();
                break;
            default:
                break;
        }
    }

    public virtual void Fly()
    {
        if((gameObject.transform.position - Owner.transform.position).magnitude > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public float Radius = .01f;
    public LayerMask hitMask;
    public LayerMask blockMask;
    public virtual void HitScan()
    {
        RaycastHit closestHit = new RaycastHit();
        Vector3 displacement = transform.position - prevPos;
        if (Physics.SphereCast(prevPos, Radius, displacement.normalized, out closestHit, displacement.magnitude, hitMask, QueryTriggerInteraction.Ignore))
        {
            Hit(closestHit);
            state = States.Hit;
        }
    }

    public virtual void Hit(RaycastHit hit)
    {
        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
        if (damageable != null) {
            damageable.Hit(this.Damage);
        }
        if (impactImage != null) { 
            var hitImage = Instantiate(impactImage, hit.point + hit.normal * .001f, Quaternion.LookRotation(hit.normal), hit.collider.transform);
            hitImage.transform.localScale = hitImage.transform.worldToLocalMatrix * Vector3.one;
        }

        ApplyForce(hit);
    }

    public virtual void ApplyForce(RaycastHit hit)
    {
        if(hit.rigidbody != null)
        {
            hit.rigidbody.AddForceAtPosition(Rigidbody.velocity, hit.point, ForceMode.Impulse);
        }
    }

    public virtual void HitEffects()
    {
        Destroy(gameObject);
    }
}
