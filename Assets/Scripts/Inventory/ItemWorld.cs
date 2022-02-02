using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.setItem(item);

        return itemWorld;
    }

    public static ItemWorld DropItem(Vector3 dropPosition, Item item)
    {
        Vector3 randomDir = UtilsClass.GetRandomDir();
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir * 1.2f, item);
        itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 5f, ForceMode2D.Impulse);
        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro textAmount;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textAmount = transform.Find("textAmount").GetComponent<TextMeshPro>();
    }

    public void setItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        if(item.amount > 1)
        {
            textAmount.SetText(item.amount.ToString());
        }
        else
        {
            textAmount.SetText("");
        }
    }

    public Item getItem()
    {
        return item;
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }


}
