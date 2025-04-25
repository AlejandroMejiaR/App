using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("References")]
    public Game3Manager gameManager;
    
    [Header("Status Indicators (Now Sliders)")]
    public Slider techAdaptabilitySlider;  // Cambiado de Image a Slider
    public Slider operationalEfficiencySlider;
    public Slider customerSatisfactionSlider;
    public TextMeshProUGUI budgetText;
    
    [Header("Status Colors")]
    public Color goodStatusColor = new Color(0.2f, 0.8f, 0.2f);
    public Color warningStatusColor = new Color(0.9f, 0.7f, 0.1f);
    public Color criticalStatusColor = new Color(0.9f, 0.1f, 0.1f);
    
    [Header("Thresholds")]
    public float warningThreshold = 40f;
    public float criticalThreshold = 20f;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<Game3Manager>();
        }
        
        UpdateUIValues();
    }

    public void UpdateUIValues()
    {
        if (gameManager == null) return;
        
        // Actualizar Sliders (valor entre 0 y 1)
        techAdaptabilitySlider.value = gameManager.techAdaptability / 100f;
        operationalEfficiencySlider.value = gameManager.operationalEfficiency / 100f;
        customerSatisfactionSlider.value = gameManager.customerSatisfaction / 100f;
        
        budgetText.text = $"Presupuesto: {gameManager.budget}€";
        
        // Actualizar colores
        UpdateStatusColor(techAdaptabilitySlider, gameManager.techAdaptability);
        UpdateStatusColor(operationalEfficiencySlider, gameManager.operationalEfficiency);
        UpdateStatusColor(customerSatisfactionSlider, gameManager.customerSatisfaction);
    }

    private void UpdateStatusColor(Slider slider, float value)
    {
        if (slider == null || slider.fillRect == null) return;
        
        Image fillImage = slider.fillRect.GetComponent<Image>();
        if (fillImage == null) return;
        
        if (value <= criticalThreshold)
        {
            fillImage.color = criticalStatusColor;
        }
        else if (value <= warningThreshold)
        {
            fillImage.color = warningStatusColor;
        }
        else
        {
            fillImage.color = goodStatusColor;
        }
    }
}