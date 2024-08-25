namespace TradeCore.AuthService.Models.Request.Query
{
    public class SelectionItem
    {
        public SelectionItem()
        {

        }

        public SelectionItem(dynamic id, string label)
        {
            Id = id;
            Label = label;
        }

        public dynamic Id { get; set; }
        public string ParentId { get; set; }
        public string Label { get; set; }
        public bool IsDisabled { get; set; }
    }
}
