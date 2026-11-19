using System;

namespace FancyDressHireSystem.Core.Models
{
    /// <summary>
    /// The customer who hires the costumes from Grand Atelier
    /// </summary>
    public class Customer
    {
        private string _customerId = string.Empty;
        private string _fullName = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        /// <summary>
        /// Creates a customer
        /// </summary>
        public Customer(string customerId, string fullName, string email, string phone)
        {
            CustomerId = customerId;
            FullName = fullName;
            Email = email;
            Phone = phone;
        }
        /// <summary>
        /// A unique customer Id and it cannot be empty
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
        /// Customer's full name and it cannot be empty
        /// </summary>
        public string FullName
        {
            get => _fullName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Full name cannot be empty.");
                _fullName = value.Trim();
            }
        }
        /// <summary>
        /// The email address of the customer.
        /// It must contain the @ sign
        /// </summary>

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
                    throw new ArgumentException("Email must be a valid address.");
                _email = value.Trim();
            }
        }
        
        /// <summary>
        /// The phone number of the customer
        /// it must have 7-15 characters and contain only digits
        /// it cannot be empty
        /// </summary>
        public string Phone
        {
            get => _phone;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Phone cannot be empty.");
                string phone = value.Trim();
                int phoneLength = phone.Length;
                if (phoneLength < 7 || phoneLength > 15)
                    throw new ArgumentException("Phone number must be between 7 and 15 characters long.");
                foreach( char number in phone)
                {
                    if (!char.IsDigit(number))
                        throw new ArgumentException("Phone number must contain digits only.");
                }
                _phone = phone;
                
            }
        }
    }
}