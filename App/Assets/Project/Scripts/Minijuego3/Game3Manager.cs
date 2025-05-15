using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [HideInInspector] public int techAdaptabilityImpact;
    [HideInInspector] public int operationalEfficiencyImpact;
    [HideInInspector] public int customerSatisfactionImpact;

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
    public GameObject problemsPanel;
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

    [Header("3D Buttons (asigna en el Inspector)")]
    public List<InteractiveButton3D> solutionCubes;      // Los 3 cubos de solución
    public InteractiveButton3D continueCube;             // El cubo “Continue”
    public TextMeshProUGUI continueCubeText;                 // El texto 3D que dice “Continuar” / “Reintentar” / “Regresar”
    public MeshRenderer continueCubeRenderer;            // Para cambiarle el color al cubo

    [Header("Colores Continue")]
    public Color defaultContinueColor = Color.white;
    public Color gameOverContinueColor = Color.red;
    public Color victoryContinueColor = Color.green;

    private bool gameEnded = false;

    private void Start()
    {
        InitializeBusinessProblems();
        SetupForProblem();
    }

    private void SetupForProblem()
    {
        // UI Panels
        problemsPanel.SetActive(true);
        resultsPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);

        // Reset botones
        foreach (var btn in solutionCubes)
        {
            btn.isButtonInteractable = true;
        }

        continueCube.isButtonInteractable = false;
        continueCubeText.text = "Continuar";
        continueCubeRenderer.material.color = defaultContinueColor;

        // Presentar el primer (o siguiente) problema
        PresentProblem();
    }

    private void InitializeBusinessProblems()
    {
        // Problema 1: Gestión de inventario
        BusinessProblem inventory = new BusinessProblem
        {
            description = "Los clientes no reciben respuestas rápidas. La empresa necesita optimizar su canal de atención digital. ¿Cómo gestionas el problema de atención al cliente?",
            digitalSolution = new Solution
            {
                description = "A) Instalar un chatbot con IA ($3500)",
                techAdaptabilityImpact = 15,
                operationalEfficiencyImpact = 20,
                customerSatisfactionImpact = 10,
                budgetImpact = -3500
            },
            intermediateSolution = new Solution
            {
                description = "B) Sistema de turnos con recordatorios ($2000)",
                techAdaptabilityImpact = -10,
                operationalEfficiencyImpact = -5,
                customerSatisfactionImpact = 25,
                budgetImpact = -2000
            },
            traditionalSolution = new Solution
            {
                description = "C) Contratar más personal telefónico ($1000)",
                techAdaptabilityImpact = -10,
                operationalEfficiencyImpact = -5,
                customerSatisfactionImpact = 25,
                budgetImpact = -1000
            }
        };
        businessProblems.Add(inventory);

        // Problema 2: Atención al cliente
        BusinessProblem customerService = new BusinessProblem
        {
            description = "Tu tienda física funciona, pero no estás vendiendo nada en línea. Estás perdiendo un segmento enorme del mercado digital.¿Cómo digitalizas tu canal de ventas?",
            digitalSolution = new Solution
            {
                description = "A) Crear un e-commerce propio con pasarela de pagos ($5000)",
                techAdaptabilityImpact = 25,
                operationalEfficiencyImpact = 25,
                customerSatisfactionImpact = 10,
                budgetImpact = -5000
            },
            intermediateSolution = new Solution
            {
                description = "B) Vender por plataformas externas (Rappi, Instagram Shop) ($2500)",
                techAdaptabilityImpact = 10,
                operationalEfficiencyImpact = 15,
                customerSatisfactionImpact = 5,
                budgetImpact = -2500
            },
            traditionalSolution = new Solution
            {
                description = "C) Tomar pedidos por WhatsApp con catálogo PDF ($1000)",
                techAdaptabilityImpact = -10,
                operationalEfficiencyImpact = 5,
                customerSatisfactionImpact = 20,
                budgetImpact = -1000
            }
        };
        businessProblems.Add(customerService);

        // Problema 3: Marketing y ventas
        BusinessProblem marketing = new BusinessProblem
        {
            description = "Tus procesos internos están desorganizados. Cada equipo usa sus propios métodos y la información no fluye bien. ¿Cómo organizas los procesos internos de la empresa? ",
            digitalSolution = new Solution
            {
                description = "A) Instalar un ERP en la nube con integración total ($6000)",
                techAdaptabilityImpact = 30,
                operationalEfficiencyImpact = 30,
                customerSatisfactionImpact = 5,
                budgetImpact = -6000
            },
            intermediateSolution = new Solution
            {
                description = "B) Usar herramientas gratuitas (Sheets, Trello) ($2000)",
                techAdaptabilityImpact = 5,
                operationalEfficiencyImpact = 15,
                customerSatisfactionImpact = 5,
                budgetImpact = -2000
            },
            traditionalSolution = new Solution
            {
                description = "C) Coordinarse por correo entre áreas ($500)",
                techAdaptabilityImpact = -15,
                operationalEfficiencyImpact = -10,
                customerSatisfactionImpact = +15,
                budgetImpact = -500
            }
        };
        businessProblems.Add(marketing);

        // Problema 4: Comunicación interna
        BusinessProblem communication = new BusinessProblem
        {
            description = "El equipo no se siente cómodo con las nuevas herramientas digitales. Algunos se resisten al cambio o prefieren los métodos anteriores. ¿Cómo enfrentas la resistencia al cambio del personal? ",
            digitalSolution = new Solution
            {
                description = "A) Implementar una plataforma de e-learning gamificada ($3000)",
                techAdaptabilityImpact = 30,
                operationalEfficiencyImpact = 10,
                customerSatisfactionImpact = 10,
                budgetImpact = -3000
            },
            intermediateSolution = new Solution
            {
                description = "B) Hacer talleres presenciales internos ($1500)",
                techAdaptabilityImpact = 10,
                operationalEfficiencyImpact = -5,
                customerSatisfactionImpact = 5,
                budgetImpact = -1500
            },
            traditionalSolution = new Solution
            {
                description = "C) Asignar compañeros expertos como apoyo informal ($0)",
                techAdaptabilityImpact = -25,
                operationalEfficiencyImpact = 5,
                customerSatisfactionImpact = -20,
                budgetImpact = 0
            }
        };
        businessProblems.Add(communication);

        // Problema 5: Análisis de datos
        BusinessProblem dataAnalysis = new BusinessProblem
        {
            description = "Tu empresa no está llegando a nuevos clientes. Dependías del voz a voz, pero ya no es suficiente para crecer. ¿Cómo mejoras el alcance y visibilidad de tu negocio?",
            digitalSolution = new Solution
            {
                description = "A) Lanzar campañas digitales automatizadas y segmentadas (Meta Ads, Google Ads) ($4500)",
                techAdaptabilityImpact = 25,
                operationalEfficiencyImpact = 20,
                customerSatisfactionImpact = 5,
                budgetImpact = -4500
            },
            intermediateSolution = new Solution
            {
                description = "B) Contratar un community manager para contenido orgánico ($2000)",
                techAdaptabilityImpact = 10,
                operationalEfficiencyImpact = 5,
                customerSatisfactionImpact = 5,
                budgetImpact = -2000
            },
            traditionalSolution = new Solution
            {
                description = "C) Imprimir volantes y hacer publicidad física local ($500)",
                techAdaptabilityImpact = -10,
                operationalEfficiencyImpact = -5,
                customerSatisfactionImpact = 20,
                budgetImpact = -500
            }
        };
        businessProblems.Add(dataAnalysis);

        // Problema 6: Protección de infomarción
        BusinessProblem dataProtection = new BusinessProblem
        {
            description = "La empresa almacena datos de clientes pero no tiene políticas de seguridad adecuadas. ¿Cómo proteges los datos sensibles de tus clientes?",
            digitalSolution = new Solution
            {
                description = "A) Implementar firewall corporativo y protocolos de ciberseguridad ($4000)",
                techAdaptabilityImpact = 25,
                operationalEfficiencyImpact = 20,
                customerSatisfactionImpact = 5,
                budgetImpact = -4000
            },
            intermediateSolution = new Solution
            {
                description = "B) Usar antivirus gratuito y crear contraseñas seguras manualmente ($1500)",
                techAdaptabilityImpact = 10,
                operationalEfficiencyImpact = 5,
                customerSatisfactionImpact = 5,
                budgetImpact = -1500
            },
            traditionalSolution = new Solution
            {
                description = "C) No hacer cambios, confiar en la responsabilidad individual ($500)",
                techAdaptabilityImpact = -20,
                operationalEfficiencyImpact = -10,
                customerSatisfactionImpact = 20,
                budgetImpact = -500
            }
        };
        businessProblems.Add(dataProtection);
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

    private void DisableCubes()
    {
        // Desactivar todos los cubos (botones)
        var buttons = FindObjectsOfType<InteractiveButton3D>();
        foreach (var button in buttons)
        {
            button.DisableButton(); // Desactivamos la interacción de los cubos
        }
    }

    private void ApplySolutionEffects(Solution solution)
    {
        // ← AGREGAR ESTO ANTES DE APLICAR LOS CAMBIOS A LOS VALORES
        techAdaptabilityImpact = solution.techAdaptabilityImpact;
        operationalEfficiencyImpact = solution.operationalEfficiencyImpact;
        customerSatisfactionImpact = solution.customerSatisfactionImpact;

        techAdaptability = Mathf.Clamp(techAdaptability + solution.techAdaptabilityImpact, minAttributeValue, maxAttributeValue);
        operationalEfficiency = Mathf.Clamp(operationalEfficiency + solution.operationalEfficiencyImpact, minAttributeValue, maxAttributeValue);
        customerSatisfaction = Mathf.Clamp(customerSatisfaction + solution.customerSatisfactionImpact, minAttributeValue, maxAttributeValue);
        budget += solution.budgetImpact;


        UpdateUI();
        CheckGameState();
    }

    private void ShowResults(Solution solution)
    {
        // Actualiza los resultados
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

        // Actualizar resultados en el panel
        resultsText.text = solution.description;
        problemsPanel.SetActive(false);
        if(!gameEnded){
            resultsPanel.SetActive(true);
        }

        // Bloquea las soluciones y desbloquea Continue
        foreach (var btn in solutionCubes)
            btn.DisableButton();

        continueCube.isButtonInteractable = true;
        //continueCubeText.text = "Continuar";
        //continueCubeRenderer.material.color = defaultContinueColor;
    }

    public void ContinueToNextProblem()
    {
        currentProblemIndex++;
        if (currentProblemIndex >= businessProblems.Count)
            Victory();
        else
            PresentProblem();
    }


    // Game3Manager.cs
    public void OnContinueCubePressed()
    {
        if (!gameEnded)
        {
            ContinueToNextProblem();
            SetupForProblem();
        }
        else if (gameOverPanel.activeSelf)
        {
            RestartGame();
        }
        else if (victoryPanel.activeSelf)
        {
            GoToLobby();
        }
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

        // Mostrar Game Over
        problemsPanel.SetActive(false);
        resultsPanel.SetActive(false); // Añadir esta línea
        gameOverPanel.SetActive(true);

        // Prepara el cubo Continue como "Reintentar"
        continueCube.isButtonInteractable = true;
        continueCubeText.text = "Reintentar";
        continueCube.hoverColor = gameOverContinueColor;
    }

    private void Victory()
    {
        gameEnded = true;

        // Mostrar Victory
        problemsPanel.SetActive(false);
        resultsPanel.SetActive(false); // Añadir esta línea
        victoryPanel.SetActive(true);

        // Prepara el cubo Continue como "Regresar"
        continueCube.isButtonInteractable = true;
        continueCubeText.text = "Regresar";
        continueCube.hoverColor = victoryContinueColor;
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

    public void GoToLobby()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }

}





