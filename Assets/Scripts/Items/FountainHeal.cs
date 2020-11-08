using UnityEngine;

public class FountainHeal : ItemDrop
{
    private bool used = false;
    private Tooltip usedT;
    private Tooltip tooltip;
    public GameObject effects;
    public GameObject light;
    private Material waterTemp;

    public override void PickUp(GameObject other)
    {
        if (used) return;
        UiManager.HideInWorldTooltip();
        OnPickup?.Invoke(this);
        var instancePlayerHealth = GameManager.Instance._playerHealth;
        GameManager.Instance.AffectPlayerCurrHealth(instancePlayerHealth.maxHealth - instancePlayerHealth.currentHealth);
        AudioManager.PlaySoundAtPosition("fountainHeal", transform.position);
        used = true;
        waterTemp.SetColor("_horizonColor", new Color(0,0,0.3f, 0.5f));
        effects.SetActive(true);
        light.SetActive(false);
    }

    private void Start()
    {
        mouseOverRadius = 3f;
        waterTemp = GetComponent<MeshRenderer>().material;

        usedT = new Tooltip("Fountain <Depleted>", $"The fountain has dried up. . .");
        tooltip = new Tooltip("Fountain <Consumable>", $"Restores health to full.");
    }

    public override void ShowTooltip()
    {
        UiManager.ShowTooltip(used?usedT:tooltip,true);
        
        UiManager.currentItemDrop = this;
    }

    public override void PlayerEnterHandler(Collider other)
    {
        playerCollider = other;
        UiManager.ShowTooltip(used?usedT:tooltip);
        UiManager.currentItemDrop = this;
    }


}