using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game3Manager : MonoBehaviour
{
    [System.Serializable]
    public class BusinessProblem
    {
        public string description;
        public Solution digitalSolution;
        public Solution intermediateSolution;
        public Solution traditionalSolution;
    }

    [System.Serializable]
    public class Solution
    {
        public string description;
        public int techAdaptabilityImpact;
        public int operationalEfficiencyImpact;
        public int customerSatisfactionImpact;
        public int budgetImpact;
    }

    // Estado de la empresa
    [Header("Atributos de Empresa")]
    public int techAdaptability = 50;
    public int operationalEfficiency = 50;
    public int customerSatisfaction = 50;
    public int budget = 1000;

    [Header("Límites de Atributos")]
    public int minAttributeValue = 0;
    public int maxAttributeValue = 100;
    public int minBudgetToSurvive = 100;

    [Header("Problemas Empresariales")]
    public List<BusinessProblem> businessProblems = new List<BusinessProblem>();
    private int currentProblemIndex = 0;

    [Header("UI Referencias")]
    public TextMeshProUGUI problemDescriptionText;
    public TextMeshProUGUI solutionAText;
    public TextMeshProUGUI solutionBText;
    public TextMeshProUGUI solutionCText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public GameObject victoryPanel;
    public GameObject resultsPanel;
    public TextMeshProUGUI resultsText;
    
    [Header("UI Controller")]
    public UIController uiController; // Nueva referencia

    private bool gameEnded = false;

    private void Start()
    {
        InitializeBusinessProblems();
        UpdateUI();
        PresentProblem();
    }

    private void InitializeBusinessProblems()
    {
        // Problema 1: Gestión de inventario
        BusinessProblem inventory = new BusinessProblem
        {
            description = "La empresa necesita mejorar su sistema de gestión de inventario. ¿Qué solución implementarías?",
            digitalSolution = new Solution
            {
                description = "Implementar un sistema ERP completo con módulos de IA para predicción de inventario",
                techAdaptabilityImpact = 30,
                operationalEfficiencyImpact = 25,
                customerSatisfactionImpact = 15,
                budgetImpact = -500
            },
            intermediateSolution = new Solution
            {
                description = "Adoptar un software de gestión de inventario en la nube con actualización en tiempo real",
                techAdaptabilityImpact = 20,
                operationalEfficiencyImpact = 20,
                customerSatisfactionImpact = 10,
                budgetImpact = -300
            },
            traditionalSolution = new Solution
            {
                description = "Mejorar el sistema manual existente con hojas de cálculo más detalladas",
                techAdaptabilityImpact = 5,
                operationalEfficiencyImpact = 10,
                customerSatisfactionImpact = 5,
                budgetImpact = -100
            }
        };
        businessProblems.Add(inventory);

        // Problema 2: Atención al cliente
        BusinessProblem customerService = new BusinessProblem
        {
            description = "Los clientes se quejan de tiempos de respuesta lentos. ¿Cómo mejorarías la atención al cliente?",
            digitalSolution = new Solution
            {
                description = "Implementar un sistema de chatbots con IA y análisis de sentimientos para atención 24/7",
                techAdaptabilityImpact = 25,
                operationalEfficiencyImpact = 20,
                customerSatisfactionImpact = 20,
                budgetImpact = -450
            },
            intermediateSolution = new Solution
            {
                description = "Crear un portal de autoservicio online con tickets de soporte y FAQs interactivas",
                techAdaptabilityImpact = 15,
                operationalEfficiencyImpact = 15,
                customerSatisfactionImpact = 15,
                budgetImpact = -250
            },
            traditionalSolution = new Solution
            {
                description = "Contratar más personal de atención al cliente y establecer procesos estandarizados",
                techAdaptabilityImpact = 0,
                operationalEfficiencyImpact = 10,
                customerSatisfactionImpact = 10,
                budgetImpact = -200
            }
        };
        businessProblems.Add(customerService);

        // Problema 3: Marketing y ventas
        BusinessProblem marketing = new BusinessProblem
        {
            description = "Las ventas están estancadas y necesitamos nuevas estrategias de marketing. ¿Qué enfoque tomarías?",
            digitalSolution = new Solution
            {
                description = "Implementar una plataforma de marketing omnicanal con análisis predictivo y personalización",
                techAdaptabilityImpact = 25,
                operationalEfficiencyImpact = 15,
                customerSatisfactionImpact = 30,
                budgetImpact = -500
            },
            intermediateSolution = new Solution
            {
                description = "Crear campañas digitales en redes sociales con análisis básico de datos",
                techAdaptabilityImpact = 15,
                operationalEfficiencyImpact = 10,
                customerSatisfactionImpact = 20,
                budgetImpact = -250
            },
            traditionalSolution = new Solution
            {
                description = "Aumentar la publicidad tradicional (folletos, radio, carteles) y las promociones en tienda",
                techAdaptabilityImpact = 0,
                operationalEfficiencyImpact = 5,
                customerSatisfactionImpact = 10,
                budgetImpact = -150
            }
        };
        businessProblems.Add(marketing);

        // Problema 4: Comunicación interna
        BusinessProblem communication = new BusinessProblem
        {
            description = "La comunicación interna entre departamentos es ineficiente. ¿Qué solución implementarías?",
            digitalSolution = new Solution
            {
                description = "Implementar una plataforma colaborativa empresarial con IA para gestión del conocimiento",
                techAdaptabilityImpact = 20,
                operationalEfficiencyImpact = 25,
                customerSatisfactionImpact = 10,
                budgetImpact = -400
            },
            intermediateSolution = new Solution
            {
                description = "Adoptar herramientas de comunicación en la nube con funciones básicas de colaboración",
                techAdaptabilityImpact = 15,
                operationalEfficiencyImpact = 15,
                customerSatisfactionImpact = 5,
                budgetImpact = -200
            },
            traditionalSolution = new Solution
            {
                description = "Establecer reuniones regulares estructuradas y mejorar los canales de comunicación existentes",
                techAdaptabilityImpact = 0,
                operationalEfficiencyImpact = 10,
                customerSatisfactionImpact = 5,
                budgetImpact = -50
            }
        };
        businessProblems.Add(communication);

        // Problema 5: Análisis de datos
        BusinessProblem dataAnalysis = new BusinessProblem
        {
            description = "Necesitamos mejorar la toma de decisiones basada en datos. ¿Qué enfoque elegirías?",
            digitalSolution = new Solution
            {
                description = "Implementar una plataforma avanzada de Big Data con analítica predictiva y dashboards personalizados",
                techAdaptabilityImpact = 30,
                operationalEfficiencyImpact = 25,
                customerSatisfactionImpact = 20,
                budgetImpact = -550
            },
            intermediateSolution = new Solution
            {
                description = "Adoptar herramientas de BI (Business Intelligence) en la nube con visualizaciones básicas",
                techAdaptabilityImpact = 20,
                operationalEfficiencyImpact = 15,
                customerSatisfactionImpact = 10,
                budgetImpact = -300
            },
            traditionalSolution = new Solution
            {
                description = "Mejorar los informes manuales y contratar un analista de datos para informes periódicos",
                techAdaptabilityImpact = 5,
                operationalEfficiencyImpact = 10,
                customerSatisfactionImpact = 5,
                budgetImpact = -150
            }
        };
        businessProblems.Add(dataAnalysis);
    }

    private void PresentProblem()
    {
        if (currentProblemIndex >= businessProblems.Count)
        {
            Victory();
            return;
        }

        BusinessProblem problem = businessProblems[currentProblemIndex];
        problemDescriptionText.text = problem.description;
        solutionAText.text = problem.digitalSolution.description;
        solutionBText.text = problem.intermediateSolution.description;
        solutionCText.text = problem.traditionalSolution.description;

        resultsPanel.SetActive(false);
    }

    public void SelectSolution(int solutionIndex)
    {
        if (gameEnded) return;

        BusinessProblem currentProblem = businessProblems[currentProblemIndex];
        Solution selectedSolution;

        switch (solutionIndex)
        {
            case 0:
                selectedSolution = currentProblem.digitalSolution;
                break;
            case 1:
                selectedSolution = currentProblem.intermediateSolution;
                break;
            case 2:
                selectedSolution = currentProblem.traditionalSolution;
                break;
            default:
                return;
        }

        ApplySolutionEffects(selectedSolution);
        ShowResults(selectedSolution);
    }

    private void ApplySolutionEffects(Solution solution)
    {
        techAdaptability = Mathf.Clamp(techAdaptability + solution.techAdaptabilityImpact, minAttributeValue, maxAttributeValue);
        operationalEfficiency = Mathf.Clamp(operationalEfficiency + solution.operationalEfficiencyImpact, minAttributeValue, maxAttributeValue);
        customerSatisfaction = Mathf.Clamp(customerSatisfaction + solution.customerSatisfactionImpact, minAttributeValue, maxAttributeValue);
        budget += solution.budgetImpact;

        UpdateUI();
        CheckGameState();
    }

    private void ShowResults(Solution solution)
    {
        string results = $"<b>Resultados de tu decisión:</b>\n\n" +
                         $"• Adaptabilidad Tecnológica: {(solution.techAdaptabilityImpact >= 0 ? "+" : "")}{solution.techAdaptabilityImpact}\n" +
                         $"• Eficiencia Operativa: {(solution.operationalEfficiencyImpact >= 0 ? "+" : "")}{solution.operationalEfficiencyImpact}\n" +
                         $"• Satisfacción del Cliente: {(solution.customerSatisfactionImpact >= 0 ? "+" : "")}{solution.customerSatisfactionImpact}\n" +
                         $"• Presupuesto: {solution.budgetImpact}€\n\n" +
                         $"<b>Estado actual de la empresa:</b>\n" +
                         $"• Adaptabilidad Tecnológica: {techAdaptability}%\n" +
                         $"• Eficiencia Operativa: {operationalEfficiency}%\n" +
                         $"• Satisfacción del Cliente: {customerSatisfaction}%\n" +
                         $"• Presupuesto Restante: {budget}€";

        resultsText.text = results;
        resultsPanel.SetActive(true);
    }

    public void ContinueToNextProblem()
    {
        if (gameEnded) return;
        
        currentProblemIndex++;
        PresentProblem();
    }

    private void CheckGameState()
    {
        if (techAdaptability <= minAttributeValue || 
            operationalEfficiency <= minAttributeValue || 
            customerSatisfaction <= minAttributeValue || 
            budget < minBudgetToSurvive)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameEnded = true;
        string reason = "";

        if (techAdaptability <= minAttributeValue)
            reason = "La empresa no pudo adaptarse a los cambios tecnológicos del mercado.";
        else if (operationalEfficiency <= minAttributeValue)
            reason = "La empresa se volvió demasiado ineficiente para operar.";
        else if (customerSatisfaction <= minAttributeValue)
            reason = "La empresa perdió a todos sus clientes.";
        else if (budget < minBudgetToSurvive)
            reason = "La empresa se quedó sin fondos para operar.";

        gameOverText.text = $"¡Transformación Digital Fallida!\n\n{reason}";
        gameOverPanel.SetActive(true);
        resultsPanel.SetActive(false);
    }

    private void Victory()
    {
        gameEnded = true;
        victoryPanel.SetActive(true);
        resultsPanel.SetActive(false);
    }

    private void UpdateUI()
    {
        if (uiController != null)
        {
            uiController.UpdateUIValues();
        }
    }



    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}