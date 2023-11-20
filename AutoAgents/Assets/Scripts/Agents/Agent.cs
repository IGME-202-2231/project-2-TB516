using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    [SerializeField] private float _maxForce = 10;
    [SerializeField] private float _perlinOffset = 100;
    [SerializeField] protected PhysicsObject _physicsObject;
    [SerializeField] protected CircleCollider _agroField;

    protected float _wanderAngle = 0;
    protected Vector3 _totalForce;

    protected void Start()
    {
        _wanderAngle = Random.Range(0,360);
    }

    private void Update()
    {
        _totalForce = Vector3.zero;

        CalcSteeringForces();
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _physicsObject.Velocity);

        _totalForce = Vector3.ClampMagnitude(_totalForce, _maxForce);
        _physicsObject.ApplyForce(_totalForce);
    }

    protected abstract void CalcSteeringForces();

    protected Vector3 Seek(Vector3 targetPos, float weight = 1)
    {
        //Get desired direction, normalize and multiply by speed
        //Subtract velocity from that, then clamp it to max force
        return weight * ((targetPos - transform.position).normalized * _physicsObject.MaxSpeed) - _physicsObject.Velocity;
    }

    protected Vector3 Seek(GameObject gameObject, float weight = 1)
    {
        return Seek(gameObject.transform.position, weight);
    }

    protected Vector3 Flee(Vector3 targetPos, float weight = 1)
    {
        return weight * ((transform.position - targetPos).normalized * _physicsObject.MaxSpeed) - _physicsObject.Velocity;
    }

    protected Vector3 Flee(GameObject gameObject, float weight = 1)
    {
        return Flee(gameObject.transform.position, weight);
    }

    protected Vector3 Wander(float wanderRadius = .5f, float weight = 1)
    {
        Vector3 futurePos = GetFuturePosition(1);

        _wanderAngle += (0.5f - Mathf.PerlinNoise(transform.position.x * 0.1f + _perlinOffset, transform.position.y * .1f + _perlinOffset)) * Mathf.PI * Time.deltaTime;

        return Seek(new Vector3(futurePos.x + Mathf.Cos(_wanderAngle) * wanderRadius, futurePos.y + Mathf.Sin(_wanderAngle) * wanderRadius), weight);
    }

    protected Vector3 Seperate(AgentManager.Team team, float weight = 1)
    {
        Vector3 seperationForce = Vector3.zero;

        for (int i = 0; i < AgentManager.Instance.Teams[(int)team].Count; i++)
        {
            if (AgentManager.Instance.Teams[(int)team][i] == this) continue;

            float distance = Vector2.Distance(transform.position, AgentManager.Instance.Teams[(int)team][i].gameObject.transform.position);
            distance += 0.000000001f;

            seperationForce += (1/distance) * Flee(AgentManager.Instance.Teams[(int)team][i].gameObject);
        }

        return weight * seperationForce;
    }

    protected Vector3 StayInBounds(float secInAdvance = 1, float weight = 5)
    {
        Vector3 futurePos = GetFuturePosition(secInAdvance);

        if (futurePos.x <= AgentManager.ScreenMin.x ||
            futurePos.x >= AgentManager.ScreenMax.x ||
            futurePos.y <= AgentManager.ScreenMin.y ||
            futurePos.y >= AgentManager.ScreenMax.y)
        {
            return Seek(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z), weight);
        }
        return Vector3.zero;
    }

    protected Vector3 GetFuturePosition(float secInAdvance = 1)
    {
        return transform.position + (_physicsObject.Velocity * secInAdvance);
    }
}
