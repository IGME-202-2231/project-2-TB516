using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    Vector3 _position;
    Vector3 _velocity;
    Vector3 _acceleration;

    [SerializeField] float _mass = 1;
    [SerializeField] float _maxSpeed = 10;

    public float MaxSpeed => _maxSpeed;
    public Vector3 Velocity => _velocity;

    void Update()
    {
        _position = transform.position;

        _velocity += _acceleration * Time.deltaTime;
        _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);

        _position += _velocity * Time.deltaTime;

        transform.position = _position;

        _acceleration = Vector3.zero;
    }

    public void ApplyForce(Vector3 force)
    {
        _acceleration += force / _mass;
    }

    public void StopMoving()
    {
        _acceleration = Vector3.zero;
        _velocity = Vector3.zero;
    }
}
