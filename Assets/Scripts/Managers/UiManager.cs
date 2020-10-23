using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : Singleton<UiManager>
{
    [HideInInspector]
    public GameObject player;
    public SpellDisplayScript[] spellSlots;
    public HealthBar healthBar;
    public GameObject minimap;
    public GameObject pauseMenu;
    public GameObject inWorldTooltipWindow;

    private bool isPaused = false;
    private HealthScript playerHealth;
    private TooltipDisplay currentTooltipWindow;
    public static ItemDrop currentItemDrop;


    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameManager.Instance._player;
        }
        PopulateSpells();


        playerHealth = player.GetComponent<HealthScript>();
        
        healthBar.SetMaxHealth(playerHealth.maxHealth);
        
        transform.Find("QuickAssignMenu").gameObject.SetActive(true);
        transform.Find("SpellCrafting").gameObject.SetActive(true);
        ResetTooltipWindow();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth != null)
            healthBar.SetHealth(playerHealth.currentHealth, playerHealth.maxHealth);
        else
        {
            healthBar.SetHealth(0f, 0f);
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            if (currentItemDrop != null)
            {
                var temp = currentItemDrop;
                currentItemDrop = null;
                temp.GetComponent<ItemDrop>().PickUp(GameManager.Instance._player);
            }
        }
    }

    public void PopulateSpells()
    {
        var spells = player.GetComponent<SpellCaster>().spells;
        for (var i = 0; i < spells.Length && i < spellSlots.Length; i++)
        {
            spellSlots[i].SetSpell(spells[i]);
        }
    }


    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void ToggleMinimap()
    {
        minimap.SetActive(!minimap.activeSelf);
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public static void SetTooltipWindow(TooltipDisplay tooltipDisplay)
    {
        Instance.currentTooltipWindow = tooltipDisplay;
    }

    public static void ShowTooltip(Tooltip tooltip)
    {
        Instance.currentTooltipWindow.Show(tooltip);
    }

    public static void HideTooltip()
    {
        Instance.currentTooltipWindow.Hide();
    }

    public static void ResetTooltipWindow()
    {
        Instance.currentTooltipWindow = Instance.inWorldTooltipWindow.GetComponent<TooltipDisplay>();
    }

    public static void ShowInWorldTooltip(Tooltip tooltip)
    {
        Instance.currentTooltipWindow = Instance.inWorldTooltipWindow.GetComponent<TooltipDisplay>();
        Instance.inWorldTooltipWindow.SetActive(true);
        Instance.currentTooltipWindow.Show(tooltip);
    }
    
    public static void HideInWorldTooltip()
    {
        Instance.inWorldTooltipWindow.SetActive(false);
    }
}

public readonly struct Tooltip
{
    public readonly string Title;
    public readonly string Body;

    public Tooltip(string title, string body)
    {
        Title = title;
        Body = body;
    }
    
    public string ToString()
    {
        return $"{Title}\n{Body}";
    }
}
