using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FancyDressHireSystem.Core.Models;

namespace FancyDressHireSystem.Tests
{
    [TestClass]
    public class CustomerTests
    {

        //  TEST 1: a valid customer should be created successfully 
        [TestMethod]
        public void Customer_WithValidDetails_IsCreated()
        {
            // ARRANGE + ACT: make a customer with all-valid values.
            Customer customer = new Customer(
                "C001",            // customerId
                "Emma Wilson",     // fullName
                "emma@email.com",  // email (must contain @)
                "12345678"         // phone (7-15 digits, numbers only)
            );

            // ASSERT: check the values were stored correctly.
            Assert.AreEqual("C001", customer.CustomerId);
            Assert.AreEqual("Emma Wilson", customer.FullName);
        }

        //  TEST 2: an email with no @ sign should be rejected 
        [TestMethod]
        public void Customer_WithEmailMissingAtSign_ThrowsError()
        {
            // an email must contain '@'. The test PASSES if it throws.
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                Customer customer = new Customer(
                    "C002",
                    "Emma Wilson",
                    "emmaemail.com",   // no @ sign — breaks the rule
                    "12345678"
                );
            });
        }

        // TEST 3: an empty full name should be rejected 
        [TestMethod]
        public void Customer_WithEmptyName_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // Name is "", name cannot be empty. The test PASSES if it throws.
                Customer customer = new Customer(
                    "C003", "", "emma@email.com", "12345678"
                );
            });
        }

        //  TEST 4: a phone that is too short should be rejected 
        [TestMethod]
        public void Customer_WithTooShortPhone_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // "12345" is only 5 digits; the rule requires 7-15.
                Customer customer = new Customer(
                    "C004", "Emma Wilson", "emma@email.com", "12345"
                );
            });
        }

        //  TEST 5: a phone containing letters should be rejected 
        [TestMethod]
        public void Customer_WithLettersInPhone_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // "1234ABCD" has letters; the rule says digits only.
                Customer customer = new Customer(
                    "C005", "Emma Wilson", "emma@email.com", "1234ABCD"
                );
            });
        }

    }
}