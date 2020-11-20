using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Type type;
    public enum Type
    {
        REFLECT,
        ABSORB
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Projectile proj = collision.gameObject.GetComponent<Projectile>();
    //    if (proj != null)
    //    {
    //        // Bad missile
    //        if (proj.type == Projectile.Type.ENEMY)
    //        {
    //            if (type == Type.REFLECT)
    //            {
    //                // get the point of contact
    //                ContactPoint contact = collision.contacts[0];

    //                // reflect our old velocity off the contact point's normal vector
    //                Vector3 reflectedVelocity = Vector3.Reflect(collision.rigidbody.velocity, contact.normal);

    //                // assign the reflected velocity back to the rigidbody
    //                collision.rigidbody.velocity = reflectedVelocity;
    //                // rotate the object by the same ammount we changed its velocity
    //                Quaternion rotation = Quaternion.FromToRotation(collision.rigidbody.velocity, reflectedVelocity);
    //                transform.rotation = rotation * transform.rotation;
    //                proj.type = Projectile.Type.FRIENDLY;
    //            }
    //        }

    //    }
    //}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile proj = collision.GetComponent<Projectile>();
        if (proj != null)
        {
            // Bad missile
            if (proj.type == Projectile.Type.ENEMY)
            {
                if (type == Type.REFLECT)
                {
                    // Get projectile information
                    Vector3 target = proj.target;
                    //float speed = proj.projectileSpeed;
                    float dist = proj.projectileDistance;

                    // Give information to projectile
                    //proj.target = Vector3.Reflect(collision.GetComponent<Rigidbody2D>().velocity, collision.transform.position) * dist;
                    proj.target = -target;
                    proj.projectileDistance += dist;
                    proj.type = Projectile.Type.FRIENDLY;
                }
            }

        }
    }
}
