using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStarts target;
    [SerializeField] private float speed;
    private int damage;
    private bool ttiggered;

    private Animator animator;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetupThunder(int _damage, CharacterStarts _target)
    {
        this.damage = _damage;
        this.target = _target;
    }
    // Update is called once per frame
    void Update()
    {
        if (!target) return;
        if (ttiggered) return;

        transform.position =  Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - target.transform.position;
        if (Vector2.Distance(transform.position, target.transform.position) < 1f)
        {
            animator.transform.localPosition = new Vector2(.3f, .2f);
            animator.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            animator.SetTrigger("hit");
            ttiggered = true;
            

            Invoke("DamageOnTarger", .2f);
        }
    }

    private void DamageOnTarger()
    {
        target.ApplyShock(true);
        target.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }
}
