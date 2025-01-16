using UnityEngine;
using Yushan.Enums;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        
    }

    protected override void Die()
    {
        base.Die();
        player.Die();
        
        GetComponent<PlayerDrop>().GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        player.stateMachine.ChangeState(player.hurtState);
        ItemDataEquipment currentArmor = Inventory.instance.GetEquipmentByType(EquitmentType.Armor);
        if (currentArmor != null)
        {
            currentArmor.ItemEffect(player.transform);
        }
    }
}
