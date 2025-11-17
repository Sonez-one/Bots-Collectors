using TMPro;
using UnityEngine;

public class CollectedResourcesCountViewer : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TextMeshProUGUI _goldCount;
    [SerializeField] private TextMeshProUGUI _ironCount;
    [SerializeField] private TextMeshProUGUI _woodCount;

    private void OnEnable()
    {
        _base.GoldValueChanged += UpdateGoldValue;
        _base.IronValueChanged += UpdateIronValue;
        _base.WoodValueChanged += UpdateWoodValue;
    }

    private void OnDisable()
    {
        _base.GoldValueChanged -= UpdateGoldValue;
        _base.IronValueChanged -= UpdateIronValue;
        _base.WoodValueChanged -= UpdateWoodValue;
    }

    private void UpdateGoldValue(int gold)
    {
        _goldCount.text = gold.ToString();
    }

    private void UpdateIronValue(int iron)
    {
        _ironCount.text = iron.ToString();
    }

    private void UpdateWoodValue(int wood)
    {
        _woodCount.text = wood.ToString();
    }
}