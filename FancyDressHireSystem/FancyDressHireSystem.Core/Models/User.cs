using System;

namespace FancyDressHireSystem.Core.Models
{
    /// <summary>
    /// A system where a staff can log in as Admin or Staff.
    /// </summary>
    public class User
    {
        //Private fields
        private string  _userId = string.Empty;
        private string  _userName = string.Empty;
        private string _passwordHash = string.Empty;
        private string _passwordSalt = string.Empty;
        private string _role = string.Empty;
        
        // Constructor
        /// <summary>
        /// Creates a user
        /// </summary>
        public User( string userId, string userName, string passwordHash, string passwordSalt, string role)
        {
            UserId = userId;
            UserName = userName;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            UserRole = role;
            
        }
        /// <summary>
        /// Unique user code.
        /// It cannot be empty
        /// </summary>
        public string UserId
        {
            get => _userId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("User ID cannot be empty.");
                _userId = value.Trim();
            }
        }
        /// <summary>
        /// The login name
        /// It cannot be empty
        /// </summary>
        public string UserName
        {
            get => _userName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("User Name cannot be empty.");
                string userName = value.Trim();
                int userNameLength = userName.Length;
                if (userNameLength < 3 || userNameLength > 20)
                    throw new ArgumentException("User Name must be between 3 and 20 characters long.");
                _userName = userName;
                
            }
        }
        /// <summary>
        /// Hashed password
        /// Never the plain password
        /// It cannot be empty
        /// </summary>
        public string PasswordHash
        {
            get => _passwordHash;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password Hash cannot be empty.");
                _passwordHash = value;
            }
        }
        /// <summary>
        /// Random salt combined with the password before hashing.
        /// It cannot be empty.
        /// </summary>
        public string PasswordSalt
        {
            get => _passwordSalt;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password Salt cannot be empty.");
                _passwordSalt = value;
            }
        }
        /// <summary>
        /// User's role: Admin or Staff.
        /// </summary>
        public string UserRole
        {
            get => _role;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Role cannot be empty.");
                string role = value.Trim();
                if (role != "Admin"  && role != "Staff")
                    throw new ArgumentException("Role must be Admin or Staff.");
                _role = role;
                    
                
            }
        }
        
        
        
    }
    

}