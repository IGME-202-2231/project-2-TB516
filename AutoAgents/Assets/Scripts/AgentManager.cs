using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AgentManager : Singleton<AgentManager>
{
    public static Vector2 ScreenMax
    {
        get
        {
            return new(
                Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect,
                Camera.main.transform.position.y + Camera.main.orthographicSize
                );
        }
    }
    public static Vector2 ScreenMin
    {
        get
        {
            return new(
                Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect,
                Camera.main.transform.position.y - Camera.main.orthographicSize
                );
        }
    }

    public enum Team
    {
        Red,
        Blue
    }

    [SerializeField] Bomber _bomberPrefab;
    [SerializeField] Fighter _fighterPrefab;
    [SerializeField] CircleCollider _obsticalPrefab;
    [SerializeField] uint _bomberCount;
    [SerializeField] uint _fighterCount;
    [Range(0, 5)][SerializeField] uint _maxObsticles = 3;

    private List<List<Agent>> _teams = new();
    private List<FlockManager> _teamFlocks = new();
    private List<CircleCollider> _obsticals = new();
    private List<Agent> _markedForDestruction = new();

    public List<List<Agent>> Teams => _teams;
    public List<CircleCollider> Obsticles => _obsticals;
    public List<FlockManager> TeamFlocks => _teamFlocks;

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            _teams.Add(new());
            _teamFlocks.Add(new());
        }

        for (int i = 0; i < _bomberCount; i++)
        {
            SpawnBomber(Team.Red);
            SpawnBomber(Team.Blue);
            
        }
        for (int i = 0; i < _fighterCount; i++)
        {
            SpawnFighter(Team.Red);
            SpawnFighter(Team.Blue);
        }
    }
    private void Update()
    {
        UpdateAgentsStates();
        CollisionCheck();

        DestroyCollidedAgents();
    }

    private void SpawnBomber(Team team)
    {
        Bomber bomber = Instantiate(_bomberPrefab);
        if (team == Team.Red)
        {
            bomber.transform.position = new Vector2(4, 4);
            bomber.Renderer.color = new(128, 0, 0);
        }
        else
        {
            bomber.transform.position = new Vector2(-4, -4);
            bomber.Renderer.color = new(0, 50, 128);
        }
        bomber.Team = team;
        

        _teams[(int)team].Add(bomber);
        _teamFlocks[(int)team].AddToFlock(bomber);
    }
    private void SpawnFighter(Team team)
    {
        Fighter fighter = Instantiate(_fighterPrefab);
        if (team == Team.Red)
        {
            fighter.transform.position = new Vector2(4, 4);
            fighter.Renderer.color = new(128, 0, 0);
        }
        else
        {
            fighter.transform.position = new Vector2(-4, -4);
            fighter.Renderer.color = new(0, 50, 128);
        }
        fighter.Team = team;

        _teams[(int)team].Add(fighter);
        _teamFlocks[(int)team].AddToFlock(fighter);
    }
    public void SpawnObsticle(Vector2 pos)
    {
        CircleCollider obsticle = Instantiate(_obsticalPrefab);
        obsticle.transform.position = pos;
        _obsticals.Add(obsticle);

        if (_obsticals.Count >= _maxObsticles && _obsticals.Count != 0)
        {
            Destroy(_obsticals[0].gameObject);
            _obsticals.RemoveAt(0);
        }
    }
    private void UpdateAgentsStates()
    {
        for (int i = 0; i < _teams.Count; i++)
        {
            for (int j = 0; j < _teams[i].Count; j++)
            {
                UpdateAgentAgro(_teams[i][j], (Team)((i + 1) % 2));
            }
        }
    }
    private void UpdateAgentAgro(Agent a, Team team)
    {
        for (int i = 0; i < _teams[(int)team].Count; i++)
        {
            if (a.AgroField.ContainsPoint(_teams[(int)team][i].transform.position) && (a is Bomber))
            {
                (a as Bomber).State = Bomber.BomberState.Scatter;
                return;
            }
            else if (a.AgroField.ContainsPoint(_teams[(int)team][i].transform.position) && (a is Fighter))
            {
                (a as Fighter).State = Fighter.FighterState.Combat;
                return;
            }
        }

        if (a is Bomber)
        {
            (a as Bomber).State = Bomber.BomberState.Formation;
        }
        else if (a is Fighter)
        {
            (a as Fighter).State = Fighter.FighterState.Formation;
        }
    }
    public void CollisionCheck()
    {
        for (int i = 0; i < _teams.Count; i++)
        {
            for (int j = 0; j < _teams[i].Count; j++)
            {
                if (_markedForDestruction.Contains(_teams[i][j])) continue;
                CheckCollision(_teams[i][j], (Team)((i + 1) % 2));
                CheckCollision(_teams[i][j]);
            }
        }
    }
    /// <summary>
    /// Agent against agents of a team
    /// </summary>
    /// <param name="a"></param>
    /// <param name="t"></param>
    private void CheckCollision(Agent a, Team t)
    {
        for (int i = 0; i < _teams[(int)t].Count; i++)
        {
            if (a.Hitbox.IsCollidingWith(_teams[(int)t][i].Hitbox))
            {
                _markedForDestruction.Add(a);
                _markedForDestruction.Add(_teams[(int)t][i]);
            }
        }
    }
    /// <summary>
    /// Agent against obsticles
    /// </summary>
    /// <param name="a"></param>
    private void CheckCollision(Agent a)
    {
        for (int i = 0; i < _obsticals.Count; i++)
        {
            if (a.Hitbox.IsCollidingWith(_obsticals[i]))
            {
                _markedForDestruction.Add(a);
            }
        }
    }
    private void DestroyCollidedAgents()
    {
        for (int i = _markedForDestruction.Count - 1; i >= 0; i--)
        {
            _teams[(int)_markedForDestruction[i].Team].Remove(_markedForDestruction[i]);
            _teamFlocks[(int)_markedForDestruction[i].Team].Flock.Remove(_markedForDestruction[i]);
            Destroy(_markedForDestruction[i].gameObject);
            _markedForDestruction.RemoveAt(i);
        }
    }
}
