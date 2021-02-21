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
        float distance = Vector2.Distance(origin, target);
        float force = distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / 9.81f);

        float velocityX = Mathf.Sqrt(force) * Mathf.Cos(angle * Mathf.Deg2Rad);
        float velocityY = Mathf.Sqrt(force) * Mathf.Sin(angle * Mathf.Deg2Rad);

        float flightDuration = distance / velocityX;

        return new Vector2(velocityX, velocityY);
    }

    public Vector2 CalculatePosInTime(Vector2 origin , Vector2 target, Vector2 force, float max,float value)
    {
        float time = (target.x - origin.x) / force.x;
        time /= max;
        time *= value;

        Vector2 result = origin + force * time;
        float valueY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (force.y * time) + origin.y;
        result.y = valueY;

        return result;
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
