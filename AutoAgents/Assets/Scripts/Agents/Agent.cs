using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    [SerializeField] private float _maxForce = 10;
    [SerializeField] private float _perlinOffset = 100;
    [SerializeField] protected PhysicsObject _physicsObject;
    [SerializeField] protected CircleCollider _agroField;
    [SerializeField] protected CircleCollider _hitbox;
    [SerializeField] protected SpriteRenderer _renderer;
    [Range(0, 10000)][SerializeField] protected float _boundsForce = 100;
    [Range(2, 10)][SerializeField] protected float _wanderRadius = 2;
    [Range(.2f, 10)][SerializeField] protected float _seperateWeight = .2f;
    [Range(.1f, 100)][SerializeField] protected float _wanderWeight = 1;

    protected float _wanderAngle = 0;
    protected Vector3 _totalForce;
    protected List<CircleCollider> _foundObsticles = new();
    protected AgentManager.Team _team;

    public AgentManager.Team Team
    {
        get => _team;
        set
        {
            _team = value;
        }
    }
    public CircleCollider AgroField => _agroField;
    public CircleCollider Hitbox => _hitbox;
    public SpriteRenderer Renderer => _renderer;

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

    protected Vector3 Cohesion(float weight = 1)
    {
        return weight * Seek(AgentManager.Instance.TeamFlocks[(int)_team].Center);
    }

    protected Vector3 Alignment(float weight = 1)
    {
        return weight * (AgentManager.Instance.TeamFlocks[(int)_team].Direction * _physicsObject.MaxSpeed) - _physicsObject.Velocity;
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

    protected Vector3 AvoidObsticles(float avoidTime = 4, float weight = 5)
    {
        Vector3 totalAvoidForce = Vector3.zero;
        _foundObsticles.Clear();

        Vector3 futurePos = GetFuturePosition(avoidTime);
        float futureDistance = Vector3.Distance(transform.position, futurePos) + _hitbox.Radius;

        for (int i = 0; i < AgentManager.Instance.Obsticles.Count; i++)
        {
            totalAvoidForce += AvoidObsticle(futureDistance, AgentManager.Instance.Obsticles[i]);
        }
        
        return weight * totalAvoidForce;
    }

    private Vector3 AvoidObsticle(float futureDistance, CircleCollider obsticle)
    {
        Vector3 agentToObst = obsticle.transform.position - transform.position;

        float forwardDot = Vector3.Dot(agentToObst, transform.up);
        float rightDot = Vector3.Dot(agentToObst, transform.right);

        if (forwardDot >= -obsticle.Radius
            && forwardDot <= futureDistance + obsticle.Radius
            && Mathf.Abs(rightDot) <= _hitbox.Radius + obsticle.Radius)
        {
            _foundObsticles.Add(obsticle);

            if (rightDot > 0)
            {
                return -transform.right * _maxForce * (1 / agentToObst.magnitude);
            }
            else
            {
                return transform.right * _maxForce * (1 / agentToObst.magnitude);
            }
        }
        return Vector3.zero;
    }

    public Vector3 GetFuturePosition(float secInAdvance = 1)
    {
        return transform.position + (_physicsObject.Velocity * secInAdvance);
    }

    protected Agent FindClosestEnemy()
    {
        float minDist = float.MaxValue;
        Agent closest = this;

        for (int i = 0; i < AgentManager.Instance.Teams[((int)_team + 1) % 2].Count; i++)
        {
            float dist = Vector2.Distance(transform.position, AgentManager.Instance.Teams[((int)_team + 1) % 2][i].transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                closest = AgentManager.Instance.Teams[((int)_team + 1) % 2][i];
            }
        }
        return closest;
    }
    
    private void OnDrawGizmos()
    {
        //
        //  Draw safe space box
        //
        Vector3 futurePos = GetFuturePosition(4);

        float dist = Vector3.Distance(transform.position, futurePos);

        Vector3 boxSize = new Vector3(_hitbox.Radius * 2f, dist * 2, _hitbox.Radius * 2f);

        Vector3 boxCenter = Vector3.zero;
        boxCenter.y += dist;

        Gizmos.color = Color.green;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(boxCenter, boxSize);
        Gizmos.matrix = Matrix4x4.identity;

        Gizmos.DrawWireSphere(futurePos, _hitbox.Radius);


        //
        //  Draw lines to found obstacles
        //
        Gizmos.color = Color.yellow;

        foreach (CircleCollider pos in _foundObsticles)
        {
            Gizmos.DrawLine(transform.position, pos.transform.position);
        }
    }
}
