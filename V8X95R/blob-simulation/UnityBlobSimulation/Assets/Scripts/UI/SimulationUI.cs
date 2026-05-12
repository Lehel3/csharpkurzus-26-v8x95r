using TMPro;
using UnityEngine;

public class SimulationUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tickText;
    [SerializeField] private TextMeshProUGUI blobCountText;
    [SerializeField] private TextMeshProUGUI foodCountText;

    public void UpdateUI(int tick, int blobCount, int foodCount) {
        tickText.text = $"Tick: {tick}";
        blobCountText.text = $"Blobs: {blobCount}";
        foodCountText.text = $"Food: {foodCount}";
    }
}
