using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour
{
    private Trajectory trajectory;
    private Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        trajectory = GetComponent<Trajectory>();
    }


    public Vector2 CalculateForce(Vector2 origin, Vector2 target, float angle)
    {
        Vector2 direction = target - origin;
        float h = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sign(2*a));
        return velocity * direction.normalized;
    }

    public void Throw(Vector2 force)
    {
        trajectory.StopIncreaseDistance();
        rigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActiveRiggidBody()
    {
        rigidBody.isKinematic = false;
    }

    public void DesactiveRiggidBody()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = 0f;
        rigidBody.isKinematic = true;
    }

}
