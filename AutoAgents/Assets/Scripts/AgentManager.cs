using System.Collections.Generic;
using UnityEngine;

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
    public List<CircleCollider> _obsticals = new();
    private float _obsticleSpawnTime = 0;
    [Range(0, 10)][SerializeField]private float _obsticleMaxSpawnTime = 5;

    public List<List<Agent>> Teams => _teams;
    public List<CircleCollider> Obsticles => _obsticals;
    public List<FlockManager> TeamFlocks => _teamFlocks;

    private void Start()
    {
        _teams.Add(new());
        _teamFlocks.Add(new());

        for (int i = 0; i < _bomberCount; i++)
        {
            SpawnBomber(Team.Red);
        }
    }

    private void Update()
    {
        if (_maxObsticles != 0 && _obsticleSpawnTime >= _obsticleMaxSpawnTime && _obsticals.Count < _maxObsticles)
        {
            SpawnObsticle();

            _obsticleSpawnTime = 0;
        }
        else if (_maxObsticles != 0 && _obsticleSpawnTime >= _obsticleMaxSpawnTime && _obsticals.Count >= _maxObsticles)
        {
            Destroy(_obsticals[0].gameObject);
            _obsticals.RemoveAt(0);

            SpawnObsticle();

            _obsticleSpawnTime = 0;
        }

        _obsticleSpawnTime += Time.deltaTime;
    }

    private void SpawnBomber(Team team)
    {
        Bomber bomber = Instantiate(_bomberPrefab);
        bomber.transform.position = new(Random.value - .5f, Random.value - .5f);
        bomber.Team = team;

        _teams[(int)team].Add(bomber);
        _teamFlocks[(int)team].AddToFlock(bomber);
    }

    private void SpawnObsticle()
    {
        CircleCollider obsticle = Instantiate(_obsticalPrefab);
        obsticle.transform.position = new(Random.Range(ScreenMin.x, ScreenMax.x), Random.Range(ScreenMin.y, ScreenMax.y));
        _obsticals.Add(obsticle);
    }
}
