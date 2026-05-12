using BlobSimulation.Core.SimObjects;
using UnityEngine;

public abstract class SimObjectVisual : MonoBehaviour
{
    protected SimObject SimObject { get; private set; }

    // Interpolation
    private Vector3 previousPos;
    private Vector3 currentPos;

    public virtual void Initialize(SimObject simObject)
    {
        this.SimObject = simObject;

        previousPos = currentPos = ToUnityPosition(simObject.Position);
    }

    public virtual void OnTick() {
        if (SimObject == null) return;

        previousPos = currentPos;
        currentPos = ToUnityPosition(SimObject.Position);
    }

    public virtual void UpdateVisual(float interpolationFactor) {
        if (SimObject == null) return;

        transform.position = Vector3.LerpUnclamped(
            previousPos,
            currentPos,
            interpolationFactor
        );
    }

    protected Vector3 ToUnityPosition(System.Numerics.Vector2 position) {
        return new Vector3(position.X, 0, position.Y);
    }
}
