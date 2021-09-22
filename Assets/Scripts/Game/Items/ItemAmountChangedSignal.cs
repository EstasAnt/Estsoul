namespace Game.Items
{
    public struct ItemAmountChangedSignal
    {
        public string ItemId;
        public int Amount;
        
        public ItemAmountChangedSignal(string itemId, int amount)
        {
            ItemId = itemId;
            Amount = amount;
        }
    }
}