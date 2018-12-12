using System.ComponentModel;

namespace ADJ.Common
{
    public enum SchoolReviewStatus
    {
        New,
        Visible,
        Hidden
    }

    public enum PhotoType
    {
        School = 1,
        Meal
    }

    public enum KidRegistrationStatus
    {
        New,
        Contacted
    }

    public enum Currency
    {
        USD,
        VND,
    }

    public enum OrderStatus
    {
        New,
    }

    public enum Ports
    {
        [Description("Cẩm Phả")] p1,
        [Description("Hòn Gai")] p2,
    }
}
