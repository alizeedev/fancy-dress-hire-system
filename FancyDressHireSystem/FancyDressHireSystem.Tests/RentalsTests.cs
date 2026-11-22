using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FancyDressHireSystem.Core.Models;

namespace FancyDressHireSystem.Tests
{
    [TestClass]
    public class RentalTests
    {
        // TEST 1: a valid rental should be created
        [TestMethod]
        public void Rental_WithValidDetails_IsCreated()
        {
            // A hire starting today, due in 3 days, not yet returned (null).
            Rental rental = new Rental(
                "R001",                      // rentalId
                "GA001",                     // costumeId
                "C001",                      // customerId
                new DateTime(2026, 11, 22),  // startDate
                new DateTime(2026, 11, 25),  // dueDate
                null,                        // returnDate (null = not returned)
                4500m                        // totalCost
            );

            Assert.AreEqual("R001", rental.RentalId);
            // Not returned yet, so IsReturned should be false.
            Assert.IsFalse(rental.IsReturned);
        }

        // TEST 2: due date before start date should be rejected 
        [TestMethod]
        public void Rental_WithDueDateBeforeStart_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // Due date (20th) is BEFORE start date (22nd) — not allowed.
                Rental rental = new Rental(
                    "R002", "GA001", "C001",
                    new DateTime(2026, 11, 22),  // start
                    new DateTime(2026, 11, 20),  // due — earlier! should throw
                    null, 1500m
                );
            });
        }

        // TEST 3: a negative total cost should be rejected
        [TestMethod]
        public void Rental_WithNegativeCost_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                Rental rental = new Rental(
                    "R003", "GA001", "C001",
                    new DateTime(2026, 11, 22),
                    new DateTime(2026, 11, 25),
                    null,
                    -100m   // negative cost — should throw
                );
            });
        }

        // TEST 4: a returned rental reports IsReturned = true
        [TestMethod]
        public void Rental_WhenReturned_IsReturnedIsTrue()
        {
            // This rental HAS a return date, so it counts as returned.
            Rental rental = new Rental(
                "R004", "GA001", "C001",
                new DateTime(2026, 11, 22),
                new DateTime(2026, 11, 25),
                new DateTime(2026, 11, 24),  // returnDate set = returned
                4500m
            );

            Assert.IsTrue(rental.IsReturned);
        }
    }
}