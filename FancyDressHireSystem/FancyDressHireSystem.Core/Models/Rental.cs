using System;

namespace FancyDressHireSystem.Core.Models
{
    /// <summary>
    /// A record of one costume being hired by one customer
    /// </summary>
    public class Rental
    {
        // Private fields
        private string _rentalId = string.Empty;
        private string _costumeId = string.Empty;
        private string _customerId = string.Empty;
        private DateTime _startDate = DateTime.MinValue;
        private DateTime _dueDate = DateTime.MinValue;
        private DateTime? _returnDate; // Null = Has not returned yet
        private decimal _totalCost;


        //Constructor
        /// <summary>
        /// Creates a rental.
        /// StartDate is set before DueDate and ReturnDate.
        /// ReturnDate is nullable
        /// </summary>
        public Rental(string rentalId, string costumeId, string customerId, DateTime startDate, DateTime dueDate,
            DateTime? returnDate, decimal totalCost)
        {
            RentalId = rentalId;
            CostumeId = costumeId;
            CustomerId = customerId;
            StartDate = startDate; // before DueDate, which checks against it
            DueDate = dueDate;
            TotalCost = totalCost;
            ReturnDate = returnDate;
        }
        /// <summary>
        /// Unique rental code.
        /// It cannot be empty.
        /// </summary>
        public string RentalId
        {
            get => _rentalId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Rental ID cannot be empty.");
                _rentalId = value.Trim();
            }
        }
        /// <summary>
        /// ID of the costume being hired.
        /// It cannot be empty.
        /// </summary>
        public string CostumeId
        {
            get => _costumeId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Costume ID cannot be empty.");
                _costumeId = value.Trim();
            }
        }

        /// <summary>
        /// ID of the customer hiring the costume.
        /// It cannot be empty.
        /// </summary>
        public string CustomerId
        {
            get => _customerId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Customer ID cannot be empty.");
                _customerId = value.Trim();
            }
        }

        /// <summary>
        /// Date the hire begins
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = value;
        }

        /// <summary>
        /// Date the costume is due back.
        /// Cannot be before StartDate.
        /// </summary>
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if (value < _startDate)
                    throw new ArgumentException("Due date cannot be before start date.");
                _dueDate = value;
            }
        }

        /// <summary>
        /// Date the costume was actually returned, or null if not returned yet.
        /// Cannot be before StartDate.
        /// </summary>
        public DateTime? ReturnDate
        {
            get => _returnDate;
            set
            {
                if (value != null && value < _startDate)
                    throw new ArgumentException("Return date cannot be before start date.");
                _returnDate = value;
            }
        }

        /// <summary>
        /// Total cost of the hire.
        /// Cannot be negative.
        /// </summary>
        public decimal TotalCost
        {
            get => _totalCost;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Total cost cannot be negative.");
                _totalCost = value;
            }
        }
        /// <summary>
        /// True if the costume has been returned.
        /// </summary>
        public bool IsReturned => _returnDate != null;
        /// <summary>
        /// True if the costume is not returned and today is after DueDate.
        /// </summary>
        public bool IsOverdue => !IsReturned && DateTime.Today > DueDate;


    }
}