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
  }

  public enum OrderStatus
  {
    New,
    [Description("Awaiting Booking")]
    AwaitingBooking,
    [Description("Booking Made")]
    BookingMade,
    [Description("Partly Manifested")]
    PartlyManifested,
    Manifested,
    Pending
  }

  public enum ContainerStatus
  {
    Pending,
    Despatch,
    Arrived,
    [Description("DC Booking Received")]
    DCBookingReceived,
    Delivered,
  }
}
