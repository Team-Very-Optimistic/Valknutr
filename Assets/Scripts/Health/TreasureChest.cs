public class TreasureChest : HealthScript {
    public override void OnDeath()
    {
        base.OnDeath();
        GameManager.Instance.SpawnItem(transform.position);
    }
}