namespace Y.Bizz.Web.Server.Api_Models.Credentials_API_Models
{
  public class ConfirmBookCredentialsApiModel
  {
    #region Public Properties
    /// <summary>
    /// Update password models.
    /// </summary>
    public int BookId { get; set; }
    public int UserId { get; set; }
    public int ConfirmCode { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Default constructor
    /// </summary>
    public ConfirmBookCredentialsApiModel()
    {

    }
    #endregion
  }
}
