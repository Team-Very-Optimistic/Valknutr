using UnityEngine;

public class HealthScriptTeleport : HealthScript
{
    private TeleportEnemy _enemy;
    private Transform _player;

    public override void Start()
    {
        base.Start();
        _player = GameManager.Instance._player.transform;
        _enemy = GetComponent<TeleportEnemy>();
    }

    public override void ApplyDamage(float damage)
    {
        //+z = looking up -- y = 0
        //-z = looking down -- y = 180
        //-x = looking right -- y = 90
        //+x = looking left -- y = -90
        //todo: this shit
        var direc = (_player.position - transform.position).normalized;
        var localRotationNormalized = Mathf.PI *(_player.eulerAngles.y) / 180f  - Mathf.Acos(direc.z) - Mathf.Asin(direc.x);
        Debug.Log(localRotationNormalized);
        if(localRotationNormalized > 0.1f)
            _enemy.Teleport();
        else
        {
            base.ApplyDamage(damage);
        }

       
    }
}