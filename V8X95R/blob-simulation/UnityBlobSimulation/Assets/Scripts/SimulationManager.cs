using BlobSimulation.Core;
using BlobSimulation.Core.SimObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance { get; private set; }

    [SerializeField] private int preferredFrameRate = 60;

    [Header("References")]
    [SerializeField] private SimulationConfigAsset configAsset;
    [SerializeField] private SimulationUI simulationUI;

    public SimulationEngine simulation;

    [Header("Prefabs")]
    [SerializeField] private GameObject blobPrefab;
    [SerializeField] private GameObject foodPrefab;

    [Header("Runtime Settings")]
    [SerializeField] private float tickRate = 20f;
    private float tickTimer;

    [SerializeField] private bool fastMode;
    [SerializeField] private int fastTicksPerFrame = 30;

    private readonly Dictionary<int, SimObjectVisual> visuals = new();

    public bool IsSimulationRunning { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = preferredFrameRate;
    }

    private void Start()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        Config config = configAsset.ToCoreConfig();
        UnityLogger logger = new UnityLogger();

        simulation = new SimulationEngine(config, logger, false);
        simulation.OnSimObjectCreated += Simulation_OnSimObjectCreated;
        simulation.OnSimObjectDestroyed += Simulation_OnSimObjectDestroyed;

        simulation.Initialize();
        RefreshUI();
    }

    private bool ValidateReferences()
    {
        if (configAsset == null)
        {
            Debug.LogError("SimulationConfigAsset is not assigned on SimulationManager.");
            return false;
        }

        if (simulationUI == null)
        {
            Debug.LogError("SimulationUI is not assigned on SimulationManager.");
            return false;
        }

        return true;
    }

    private void Update()
    {
        if (!IsSimulationRunning)
        {
            return;
        }

        if (fastMode)
        {
            RunFastSimulation();
            return;
        }

        RunRealtimeSimulation();
    }

    private void RunRealtimeSimulation()
    {
        float safeTickRate = Mathf.Max(tickRate, 0.001f);
        float tickInterval = 1f / safeTickRate;
        tickTimer += Time.deltaTime;

        while (tickTimer >= tickInterval)
        {
            AdvanceOneTick(runVisualTick: true);
            tickTimer -= tickInterval;
        }

        float interpolationFactor = tickTimer / tickInterval;
        SyncVisuals(interpolationFactor);
    }

    private void RunFastSimulation()
    {
        int steps = Mathf.Max(1, fastTicksPerFrame);

        for (int i = 0; i < steps; i++)
        {
            AdvanceOneTick(runVisualTick: false);
        }

        SyncVisuals(1f);
    }

    private void AdvanceOneTick(bool runVisualTick)
    {
        simulation.Step();

        if (runVisualTick)
        {
            foreach (SimObjectVisual visual in visuals.Values)
            {
                visual.OnTick();
            }
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        simulationUI.UpdateUI(
            simulation.Tick,
            simulation.World.GetBlobs.Count(),
            simulation.World.GetFoods.Count());
    }

    private void SyncVisuals(float interpolationFactor)
    {
        foreach (SimObjectVisual visual in visuals.Values)
        {
            visual.UpdateVisual(interpolationFactor);
        }
    }

    #region --- Simulation Controls ---
    public void StartSimulation()
    {
        IsSimulationRunning = true;
    }

    public void StopSimulation()
    {
        IsSimulationRunning = false;
    }

    public void StepSimulation()
    {
        if (IsSimulationRunning)
        {
            Debug.LogWarning("Cannot manually step simulation while it's running.");
            return;
        }

        AdvanceOneTick(runVisualTick: true);
        SyncVisuals(1f);
    }
    #endregion


    #region --- Simulation Event Handlers ---
    private void Simulation_OnSimObjectCreated(SimObject simObject)
    {
        GameObject prefab = GetPrefabFor(simObject);

        if (prefab == null)
        {
            Debug.LogWarning($"No prefab registered for sim object type: {simObject.GetType().Name}");
            return;
        }

        Vector3 spawnPos = new Vector3(simObject.Position.X, 0f, simObject.Position.Y);
        GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity);

        SimObjectVisual visual = go.GetComponent<SimObjectVisual>();
        if (visual == null)
        {
            Debug.LogWarning($"Prefab '{prefab.name}' has no SimObjectVisual component.");
            Destroy(go);
            return;
        }

        visual.Initialize(simObject);
        visuals[simObject.Id] = visual;
    }

    private GameObject GetPrefabFor(SimObject simObject)
    {
        return simObject switch
        {
            Blob => blobPrefab,
            Food => foodPrefab,
            _ => null
        };
    }

    private void Simulation_OnSimObjectDestroyed(int simObjectId)
    {
        if (visuals.TryGetValue(simObjectId, out SimObjectVisual visual))
        {
            Destroy(visual.gameObject);
            visuals.Remove(simObjectId);
        }
    }
    #endregion


    private void OnDestroy()
    {
        if (simulation != null)
        {
            simulation.OnSimObjectCreated -= Simulation_OnSimObjectCreated;
            simulation.OnSimObjectDestroyed -= Simulation_OnSimObjectDestroyed;
        }

        if (Instance == this)
        {
            Instance = null;
        }
    }
}
