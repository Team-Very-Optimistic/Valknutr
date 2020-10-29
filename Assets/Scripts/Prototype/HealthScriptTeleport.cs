using UnityEngine;

public class HealthScriptTeleport : HealthScript
{
    private TeleportEnemy _enemy;
    private Transform _player;
    private Vector3 targetPoint;
    [SerializeField]
    protected float visionCone = 45f;

    public override void Start()
    {
        base.Start();
        _player = GameManager.Instance._player.transform;
        _enemy = GetComponent<TeleportEnemy>();
    }

    public override bool ApplyDamage(float damage, Color color)
    {
        //+z = looking up -- y = 0
        //-z = looking down -- y = 180
        //-x = looking right -- y = 90
        //+x = looking left -- y = -90
        //todo: this shit
        // var direc = (_player.position - transform.position).normalized;
        // var localRotationNormalized = Mathf.PI *(_player.eulerAngles.y) / 180f  - Mathf.Acos(direc.z) - Mathf.Asin(direc.x);
        var localRotationNormalized = (_enemy.transform.rotation.eulerAngles - _enemy.targetRotation.eulerAngles).magnitude;
        if(localRotationNormalized < visionCone)
            _enemy.Teleport();
        else
        {
            return base.ApplyDamage(damage);
        }

        return false;
    }
}