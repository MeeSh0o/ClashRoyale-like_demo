using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject Target;
    public float speed;
    public int damage;
    public Rigidbody rb;
    public Vector3 offset;


    /// <summary>
    /// 阵营
    /// </summary>
    public string Fold
    {
        get
        {
            return gameObject.tag;
        }
        set
        {
            gameObject.tag = value;
        }
    }
    private void Update()
    {
        // 方向修正
        Vector2 disXZ = new Vector2(Target.transform.position.x, Target.transform.position.z) - new Vector2(transform.position.x, transform.position.z) + new Vector2(offset.x, offset.z);
        Vector2 targetV = disXZ.normalized * speed;

        Vector2 vXZnow = new Vector2(rb.velocity.x, rb.velocity.z);
        Vector2 temp = (targetV - vXZnow);

        while (Vector2.Angle(vXZnow + temp, vXZnow) > 45 * Time.deltaTime)
        {
            temp *= 0.5f;
        }

        rb.velocity = new Vector3(vXZnow.x + temp.x, rb.velocity.y, vXZnow.y + temp.y);
        transform.LookAt(transform.position + rb.velocity);

        if (transform.position.y < -100)
        {
            Destroy(gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!collision.gameObject.CompareTag(Fold))
    //    {
    //        Unit enemy = collision.transform.parent.GetComponent<Unit>();
    //        enemy.Hit(damage);
    //        Destroy(gameObject);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(Fold))
        {
            Unit enemy = other.transform.parent.GetComponent<Unit>();
            enemy.Hit(damage);
            Destroy(gameObject);
        }
    }

    public void SetBullet(GameObject target, string fold,int damage, float speed,Vector3 offset)
    {
        Target = target;
        this.damage = damage;
        this.speed = speed;
        Fold = fold;
        gameObject.SetActive(true);

        float dis = Tools.Distance(transform, target.transform);
        float time = 0.5f * dis / this.speed;
        float velocityY = Physics.gravity.magnitude * time;

        Vector2 disXZ = new Vector2(Target.transform.position.x, Target.transform.position.z) - new Vector2(transform.position.x, transform.position.z) + new Vector2(offset.x, offset.z);
        Vector2 velocityXZ = disXZ.normalized * speed;

        rb.velocity = new Vector3(velocityXZ.x, velocityY, velocityXZ.y);
    }

    //public void OnEnable()
    //{
    //    SetBullet(Target, damage, speed, offset);
    //}
}
