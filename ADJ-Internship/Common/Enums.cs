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
    AwaitingBooking,
    BookingMade,
    PartlyManifested,
    Manifested,
    Pending
  }

  public enum ContainerStatus
  {
    Pending,
    Despatch,
    Arrived,
    DCBookingReceived,
    Delivered,
  }
}
