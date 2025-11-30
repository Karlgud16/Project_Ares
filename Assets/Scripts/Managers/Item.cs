using System.Collections;
using UnityEngine;

[System.Serializable]
public abstract class Item : IInstantiableItem
{
    public abstract string GetName();

    public virtual Sprite ItemSprite()
    {
        return Resources.Load<Sprite>("ItemSprites/HermesSwiftSandals");
    }

    public virtual void UpdateOneSecond(ItemManager player, int stacks)
    {

    }

    public virtual void UpdateOneTick(ItemManager player, int stacks)
    {

    }

    public virtual void OnHit(ItemManager player, BaseEnemyHealth enemy, int stacks)
    {

    }

    public virtual void OnDamage(ItemManager player, BaseEnemyHealth enemy, int stacks)
    {

    }

    public virtual void OnEnemyDeath(ItemManager player, BaseEnemyHealth enemy, int stacks)
    {

    }

    public virtual bool OnInstantiated(ItemManager player, int stacks)
    {

        return true;
    }

    public virtual IEnumerator Coroutine(ItemManager player, int stacks)
    {
        yield return new WaitForSeconds(1f);
    }
}

public interface IInstantiableItem 
{
    bool OnInstantiated(ItemManager player, int stacks);
}

//Additional Movement
public class HermesSwiftSandals : Item
{
    public override string GetName()
    {
        return "Hermes Swift Sandals";
    }

    public override Sprite ItemSprite()
    {
        return Resources.Load<Sprite>("ItemSprites/HermesSwiftSandals");
    }

    public override bool OnInstantiated(ItemManager player, int stacks)
    {
        player.GetComponent<PlayerManager>().DefaultPlayerMoveSpeed += 1 + (0.5f * stacks);
        return true;
    }
}

//Additional Jump Force
public class ValkyriesWingedBoots : Item 
{
    public override string GetName()
    {
        return "Valkyries Winged Boots";
    }

    public override Sprite ItemSprite()
    {
        return Resources.Load<Sprite>("ItemSprites/ValkyriesWingedBoots");
    }

    public override bool OnInstantiated(ItemManager player, int stacks)
    {
        player.GetComponent<PlayerManager>().DefaultPlayer.Jump += 1 + (0.5f * stacks);
        return true;
    }
}

//Increase Movement when player is taking damage
public class CursedSpurs : Item
{
    public override string GetName()
    {
        return "Cursed Spurs";
    }

    public override Sprite ItemSprite()
    {
        return Resources.Load<Sprite>("ItemSprites/CursedSpurs");
    }

    public override void UpdateOneTick(ItemManager player, int stacks)
    {
        if (player.GetComponent<PlayerManager>().CurrentPlayerHealth > player.GetComponent<PlayerManager>().CurrentPlayerHealth * 0.75)
        {
            player.GetComponent<PlayerManager>().DefaultPlayer.Speed = player.GetComponent<PlayerManager>().DefaultPlayerMoveSpeed * 1f;
        }
        else if (player.GetComponent<PlayerManager>().CurrentPlayerHealth < player.GetComponent<PlayerManager>().PlayerHealth * 0.75 && player.GetComponent<PlayerManager>().CurrentPlayerHealth > player.GetComponent<PlayerManager>().PlayerHealth * 0.5)
        {
            player.GetComponent<PlayerManager>().DefaultPlayer.Speed = (float)(player.GetComponent<PlayerManager>().DefaultPlayerMoveSpeed * 1.1f + (0.2 * stacks));
        }
        else if (player.GetComponent<PlayerManager>().CurrentPlayerHealth < player.GetComponent<PlayerManager>().PlayerHealth * 0.5 && player.GetComponent<PlayerManager>().CurrentPlayerHealth > player.GetComponent<PlayerManager>().PlayerHealth * 0.25)
        {
            player.GetComponent<PlayerManager>().DefaultPlayer.Speed = (float)(player.GetComponent<PlayerManager>().DefaultPlayerMoveSpeed * 1.2f + (0.2 * stacks));
        }
        else if (player.GetComponent<PlayerManager>().CurrentPlayerHealth < player.GetComponent<PlayerManager>().PlayerHealth * 0.25)
        {
            player.GetComponent<PlayerManager>().DefaultPlayer.Speed = (float)(player.GetComponent<PlayerManager>().DefaultPlayerMoveSpeed * 1.3f + (0.2 * stacks));
        }
    }
}

//Add life steal to the player
public class AnubisAnkh : Item 
{
    public override string GetName()
    {
        return "Anubis Ankh";
    }

    public override Sprite ItemSprite()
    {
        return Resources.Load<Sprite>("ItemSprites/AnubisAnkh");
    }

    public override void OnEnemyDeath(ItemManager player, BaseEnemyHealth enemy, int stacks)
    {
        if(player.GetComponent<HealthSystem>().PlayerCurrentHealth < player.GetComponent<PlayerManager>().StartHealth)
        {
            player.GetComponent<HealthSystem>().PlayerCurrentHealth += player.GetComponent<PlayerManager>().StartHealth / 10f;

            if (player.GetComponent<HealthSystem>().PlayerCurrentHealth > player.GetComponent<PlayerManager>().StartHealth)
            {
                player.GetComponent<HealthSystem>().PlayerCurrentHealth = player.GetComponent<PlayerManager>().StartHealth;
            }
        }
    }
}

//Slow down enemy when hitting the player
public class SlimeArmour : Item 
{
    public override string GetName()
    {
        return "Slime Armour";
    }

    public override Sprite ItemSprite()
    {
        return Resources.Load<Sprite>("ItemSprites/SlimeArmour");
    }

    public override IEnumerator Coroutine(ItemManager player, int stacks)
    {
        player.GetComponent<EnemyManager>().CurrentBaseEnemyMove = (float)(player.GetComponent<EnemyManager>().DefaultBaseEnemyMove * 0.5 - (0.05 * stacks));
        player.GetComponent<EnemyManager>().CurrentBruteMove = (float)(player.GetComponent<EnemyManager>().DefaultBruteMove * 0.5 - (0.05 * stacks));
        yield return new WaitForSeconds(3f);
        player.GetComponent<EnemyManager>().CurrentBaseEnemyMove = player.GetComponent<EnemyManager>().DefaultBaseEnemyMove;
        player.GetComponent<EnemyManager>().CurrentBruteMove = player.GetComponent<EnemyManager>().DefaultBruteMove;
    }
}
