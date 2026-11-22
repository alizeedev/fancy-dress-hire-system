using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FancyDressHireSystem.Core.Models;

namespace FancyDressHireSystem.Tests
{
    [TestClass]
    public class UserTests
    {
        //  TEST 1: a valid user should be created 
        [TestMethod]
        public void User_WithValidDetails_IsCreated()
        {
            User user = new User(
                "U001",        // userId
                "sakshee",     // userName (3-20 characters)
                "hashedpw",    // passwordHash
                "somesalt",    // passwordSalt
                "Admin"        // role (must be Admin or Staff)
            );

            Assert.AreEqual("U001", user.UserId);
            Assert.AreEqual("sakshee", user.UserName);
        }

        // TEST 2: a username that is too short should be rejected
        [TestMethod]
        public void User_WithTooShortUsername_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // "ab" is only 2 characters; rule requires 3-20.
                User user = new User(
                    "U002", "ab", "hashedpw", "somesalt", "Admin"
                );
            });
        }

        // TEST 3: an invalid role should be rejected 
        [TestMethod]
        public void User_WithInvalidRole_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // "Manager" is not allowed (only Admin or Staff).
                User user = new User(
                    "U003", "sakshee", "hashedpw", "somesalt", "Manager"
                );
            });
        }

        // TEST 4: an empty password hash should be rejected 
        [TestMethod]
        public void User_WithEmptyPasswordHash_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // Password hash is "" — not allowed.
                User user = new User(
                    "U004", "sakshee", "", "somesalt", "Staff"
                );
            });
        }
    }
}