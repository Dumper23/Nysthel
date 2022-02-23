using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopCustomer
{
    int BoughtItem(ShopItem.ItemType itemType);
    int BoughtItem(ItemShopItem.ItemType itemType);
    bool TrySpendGoldAmount(int goldAmount);

    float[] GetStatistics();

}
