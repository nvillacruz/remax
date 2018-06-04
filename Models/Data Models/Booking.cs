using System;
using System.ComponentModel.DataAnnotations;

namespace Y.Bizz.Web.Server
{
    public class Booking
    {
        #region Public Properties
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Booking Notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// For navigation property.
        /// </summary>
        [Required]
        public ApplicationUser User { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Booking Service ID
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// Booking start date
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Booking end date
        /// </summary>
        public DateTimeOffset EndDate { get; set; }


        /// <summary>
        /// Booking start date
        /// </summary>
        public DateTimeOffset ActualStartDate { get; set; }

        /// <summary>
        /// Booking end date
        /// </summary>
        public DateTimeOffset ActualEndDate { get; set; }


        public bool IsConfirm { get; set; }
        public DateTimeOffset ConfirmDate { get; set; }

        public bool IsCancel { get; set; }
        public DateTimeOffset CancelDate { get; set; }


        /// <summary>
        /// Booking Status
        /// </summary>
        public int Status { get; set; }
        #endregion
    }
}
