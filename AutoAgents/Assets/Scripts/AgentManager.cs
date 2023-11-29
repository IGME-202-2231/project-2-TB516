using System.Collections.Generic;
using UnityEngine;

public class AgentManager : Singleton<AgentManager>
{
    public enum Team
    {
        Red,
        Blue
    }

    [SerializeField] Bomber _bomberPrefab;
    [SerializeField] Fighter _fighterPrefab;
    [SerializeField] uint _bomberCount;
    [SerializeField] uint _fighterCount;

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

    private List<List<Agent>> _teams = new();

    public List<List<Agent>> Teams => _teams;

    private void Start()
    {
        _teams.Add(new());

        for (int i = 0; i < _bomberCount; i++)
        {
            SpawnAgent(Team.Red);
        }
    }

    private void SpawnAgent(Team team)
    {
        Bomber bomber = Instantiate(_bomberPrefab);
        bomber.transform.position = new(Random.value - .5f, Random.value - .5f);
        bomber.Team = team;

        _teams[(int)team].Add(bomber);
    }
}
