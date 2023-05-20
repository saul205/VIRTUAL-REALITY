using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public GameObject target;
    public int hp = 100;
    private bool dead = false;
    public Animator animator;
    public AudioSource attackSound;
    public AudioClip deathClip;
    public int Hp { get
        {
            return hp;
        }
        set 
        {
            hp = value;
        } 
    }
    public void Hit(int dmg)
    {
        Hp -= dmg;
        if (Hp <= 0)
            Die();
    }

    public void Die()
    {
        dead = true;
        animator.SetTrigger("Dead");
        var arrows = gameObject.GetComponentsInChildren<Shootable>();
        foreach (Shootable sh in arrows)
        {
            sh.transform.parent = null;
        }

        AudioSource.PlayClipAtPoint(deathClip, transform.position);
        Destroy(gameObject, 1.3f);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.SetFloat("MoveSpeed", 2);
    }

    public float speed = 5;
    public float rotSpeed = 90f;
    void Update()
    {
        if (dead)
            return;

        Vector3 currentDirection = transform.forward;
        Vector3 desiredDirection = (target.transform.position - transform.position).normalized;
        var a = Vector3.SignedAngle(currentDirection, desiredDirection, Vector3.up);
        var rot = (Mathf.Abs(a) / 180 + 1) * rotSpeed * Mathf.Sign(a);
        Debug.Log(rot);
        if (Mathf.Abs(a) < rot * Time.deltaTime)
            transform.LookAt(target.transform.position);
        else
        {
            transform.Rotate(transform.up, rot * Time.deltaTime);
        }
        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (dead)
            return;

        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null && damageable.gameObject.CompareTag("Player"))
        {
            attackSound.Play();
            animator.SetTrigger("Attack");
            damageable.Hit(10);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (dead)
            return;

        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null && damageable.gameObject.CompareTag("Player"))
        {
            attackSound.Play();
            animator.SetTrigger("Attack");
            damageable.Hit(10);
        }
    }
}
