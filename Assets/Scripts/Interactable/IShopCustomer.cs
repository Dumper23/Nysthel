using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopCustomer
{
    void BoughtItem(ShopItem.ItemType itemType);
    bool TrySpendGoldAmount(int goldAmount);

    float[] GetStatistics();

}
