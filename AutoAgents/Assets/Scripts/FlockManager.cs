using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlockManager
{
    List<Agent> _flock = new();
    Vector3 _center;
    Vector3 _flockDirection;

    public Vector3 Center => _center;
    public Vector3 Direction => _flockDirection;
    public List<Agent> Flock => _flock;

    // Update is called once per frame
    void Update()
    {
        _center = GetCenterPoint();
        _flockDirection = GetFlockDirection();
    }

    public void AddToFlock(Agent a)
    {
        _flock.Add(a);
        _center = GetCenterPoint();
    }

    private Vector3 GetCenterPoint()
    {
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < _flock.Count; i++)
        {
            pos += _flock[i].transform.position;
        }

        return pos / _flock.Count;
    }

    private Vector3 GetFlockDirection()
    {
        Vector3 direction = Vector3.zero;

        for (int i = 0; i < _flock.Count; i++)
        {
            direction += _flock[i].transform.up;
        }

        return direction.normalized;
    }
}
