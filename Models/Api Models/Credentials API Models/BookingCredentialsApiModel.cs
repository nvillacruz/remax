using System;

namespace Y.Bizz.Web.Server
{
  public class BookingCredentialsApiModel
  {
    #region Public Properties
    /// <summary>
    /// Update password models.
    /// </summary>
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ServiceId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }


    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }
    public DateTimeOffset CreatedBy { get; set; }
    public DateTimeOffset ModifiedBy { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Default constructor
    /// </summary>
    public BookingCredentialsApiModel()
    {

    }
    #endregion
  }
}