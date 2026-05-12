using BlobSimulation.Core.SimObjects;
using TMPro;
using UnityEngine;

public class BlobVisual : SimObjectVisual {
    private Blob Blob => (Blob)SimObject;


    [SerializeField] private TextMeshProUGUI overlayText;


    public override void Initialize(SimObject simObject) {
        base.Initialize(simObject);

        // Blob-specific setup here later
        // example: color, scale, energy display, etc.
    }

    public override void OnTick() {
        base.OnTick();

        UpdateOverlay($"{Blob.Energy.ToString("0")}");
    }

    public void UpdateOverlay(string text) {
        overlayText.text = text;
    }
}
