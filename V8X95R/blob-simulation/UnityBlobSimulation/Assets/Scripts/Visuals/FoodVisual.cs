using BlobSimulation.Core.SimObjects;
using UnityEngine;

public class FoodVisual : SimObjectVisual {
    private Food Food => (Food)SimObject;

    public override void Initialize(SimObject simObject) {
        base.Initialize(simObject);
    }
}
