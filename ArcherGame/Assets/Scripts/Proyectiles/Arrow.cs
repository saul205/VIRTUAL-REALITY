using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : ChargedProjectile, IPickup
{
    #region Pickup
    public bool CanPickup { get; set; } = false;
    public int value { get; set; } = 1;
    #endregion
    protected override void Update()
    {
        base.Update();

        switch (state)
        {
            case States.Flying:
                if(Rigidbody.velocity != Vector3.zero)
                    transform.forward = Rigidbody.velocity.normalized;
                break;
            case States.Hit:
                if(transform.parent == null)
                {
                    state = States.Fall;
                    Rigidbody.isKinematic = false;
                    gameObject.GetComponentInChildren<MeshCollider>().isTrigger = false;
                    Rigidbody.useGravity = true;
                }
                break;
            case States.Fall:
                break;
            case States.Grounded:
                break;
            default:
                break;
        }
    }

    public override void HitEffects()
    {
        return;
    }

    public override void Hit(RaycastHit hit)
    {
        gameObject.transform.SetParent(hit.collider.gameObject.transform, true);

        base.Hit(hit);

        Rigidbody.isKinematic = true;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.useGravity = false;
        transform.position = hit.point;

        CanPickup = true;

        var outline = gameObject.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = Color.red;
        outline.OutlineWidth = 5f;
    }
}
