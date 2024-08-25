namespace TradeCore.AuthService.Models.Response.Query
{
    public class GetAppOperationClaimsQueryResponse
    {
        public List<TreeView> Data { get; set; }

    }

    public partial class TreeView
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
        public List<Child> Children { get; set; }
    }

    public partial class Child
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
        public List<ChildChild> Children { get; set; }
    }

    public partial class ChildChild
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
    }
}
