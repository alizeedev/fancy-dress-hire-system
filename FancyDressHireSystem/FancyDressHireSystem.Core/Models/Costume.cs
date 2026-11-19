using System;
namespace FancyDressHireSystem.Core.Models
{
    /// <summary>
    /// A costume available to hire at the Grand Atelier
    /// </summary>
    public class Costume
    {
        //Private fields
        private string _costumeId = string.Empty;
        private string _costumeName =  string.Empty;
        private string _costumeCategory =  string.Empty;
        private string _costumeEra = string.Empty;
        private string _costumeSize = string.Empty;
        private decimal _dailyHirePrice;
        private int _totalQuantity;
        private int _availableQuantity;
        
        //Constructor
        /// <summary>
        /// Creates a costume
        /// </summary>
        public Costume(string costumeId, string costumeName, string category, string era, string size,
            decimal dailyHirePrice, int totalQuantity, int availableQuantity)
        {
            CostumeId = costumeId;
            CostumeName = costumeName;
            Category = category;
            Era = era;
            Size = size;
            DailyHirePrice = dailyHirePrice;
            TotalQuantity = totalQuantity;
            AvailableQuantity = availableQuantity;
        }
        
        //Public properties with validation
        /// <summary>
        /// Unique costume code example "GA001" and it cannot be empty
        /// </summary>
        public string CostumeId
        {
            get => _costumeId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Costume ID cannot be null or empty.");
                _costumeId = value.Trim();
            }
        }
        /// <summary>
        /// Display name of the costume and it cannot be empty
        /// </summary>
        public string CostumeName
        {
            get => _costumeName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Costume Name cannot be null or empty.");
                _costumeName = value.Trim();
            }
        }
        /// <summary>
        /// Category of each costume.
        /// it must be either Royal, Masquerade or Historical
        /// </summary>
        public string Category
        {
            get => _costumeCategory;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Costume Category cannot be null or empty.");
                string costumeCategory = value.Trim();
                if (costumeCategory != "Royal" && costumeCategory != "Historical" && costumeCategory != "Masquerade")
                    throw new ArgumentException("Category must be Royal, Historical or Masquerade.");
                _costumeCategory = costumeCategory;
                    
            }
            
        }
        /// <summary>
        /// Historical era represents the time period the costume is from. It cannot be empty
        /// </summary>
        public string Era
        {
            get => _costumeEra;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Costume Era cannot be null or empty.");
                _costumeEra = value.Trim();
            }
        }
        /// <summary>
        /// Costume size: XS, S, M, L, XL or XXL.
        /// </summary>
        public string Size
        {
            get => _costumeSize;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Costume Size cannot be null or empty.");
                string costumeSize = value.Trim();
                if (costumeSize != "XS" && costumeSize != "S" && costumeSize != "M" && costumeSize != "L" && costumeSize != "XL" && costumeSize != "XXL")
                    throw new ArgumentException("Size must be XS, S, M, L, XL or XXL.");
                _costumeSize = costumeSize;
            }
        }
        /// <summary>
        ///  Price to hire the costume for one day. Must be greater than 0.
        /// </summary>
        public decimal DailyHirePrice
        {
            get => _dailyHirePrice;
            set
            {
                
                if (value <= 0)
                    throw new ArgumentException("Daily hire price must be greater than 0.");
                _dailyHirePrice = value;
                
            }
        }
        /// <summary>
        /// Total number of costumes owned. Must be 0 or more, and not less than AvailableQuantity.
        /// </summary>
        public int TotalQuantity
        {
            get => _totalQuantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Total quantity cannot be negative.");
                if (value < _availableQuantity)
                    throw new ArgumentException("Total quantity cannot be less than available quantity.");
                _totalQuantity = value;
            }
        }
        /// <summary>
        /// Number of costumes currently available to hire. Must be between 0 and TotalQuantity.
        /// </summary>
        public int AvailableQuantity
        {
            get => _availableQuantity;
            set
            {
                if (value < 0 || value > _totalQuantity)
                    throw new ArgumentException(
                        $"Available quantity must be between 0 and {_totalQuantity}.");
                _availableQuantity = value;
            }
        }
        /// <summary>
        /// True if at least one costume is available.
        /// </summary>
        public bool IsAvailable => AvailableQuantity > 0;
    }
}