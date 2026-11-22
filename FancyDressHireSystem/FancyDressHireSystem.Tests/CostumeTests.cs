using System;                                   
using Microsoft.VisualStudio.TestTools.UnitTesting;  
using FancyDressHireSystem.Core.Models;
namespace FancyDressHireSystem.Tests
{
    // [TestClass] tells MSTest: "this class contains tests."
    [TestClass]
    public class CostumeTests
    {
        // ---- TEST 1: a valid costume should be created successfully ----
        [TestMethod]
        public void Costume_WithValidDetails_IsCreated()
        {
            // ARRANGE + ACT: make a costume with all-valid values.
            Costume costume = new Costume(
                "GA001",
                "Victorian Gown",
                "Historical",
                "Victorian",
                "M",
                1500m,
                5,
                3
            );

            // ASSERT: check the values were stored correctly.
            // Assert.AreEqual(expected, actual) passes if they match.
            Assert.AreEqual("GA001", costume.CostumeId);
            Assert.AreEqual("Victorian Gown", costume.CostumeName);
            // IsAvailable should be true because availableQuantity (3) > 0.
            Assert.IsTrue(costume.IsAvailable);
        }

        //TEST 2: a negative price should be rejected 
        // This test PASSES if the class throws the error it should.
        [TestMethod]
        public void Costume_WithNegativePrice_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // ACT: try to make a costume with a negative price.
                Costume costume = new Costume(
                    "GA002", "Royal Robe", "Royal", "Tudor", "L",
                    -50m,   // invalid: negative price
                    2, 1
                );
            });
        }
    

    // TEST 3: an empty name should be rejected
    [TestMethod]
        public void Costume_WithEmptyName_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // ACT: try to make a costume with an empty name.
                Costume costume = new Costume(
                    "GA003", "", "Historical", "Victorian", "M",
                    1000m, 2, 1
                );
            });
        }

        //  TEST 4: an invalid category should be rejected 
        [TestMethod]
        public void Costume_WithInvalidCategory_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // "Pirate" is not allowed (only Royal/Historical/Masquerade).
                Costume costume = new Costume(
                    "GA004", "Pirate Suit", "Pirate", "Golden Age", "M",
                    800m, 2, 1
                );
            });
        }

        // ---- TEST 5: an invalid size should be rejected ----
        [TestMethod]
        public void Costume_WithInvalidSize_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // "XXXL" is not a valid size.
                Costume costume = new Costume(
                    "GA005", "Giant Robe", "Royal", "Tudor", "XXXL",
                    900m, 2, 1
                );
            });
        }

        // ---- TEST 6: availableQuantity bigger than totalQuantity is rejected ----
        [TestMethod]
        public void Costume_WithAvailableMoreThanTotal_ThrowsError()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                // 5 available but only 3 total — impossible, should throw.
                Costume costume = new Costume(
                    "GA006", "Masque Gown", "Masquerade", "Baroque", "S",
                    1200m, 3, 5
                );
            });
        }

        // ---- TEST 7: a costume with 0 available is NOT available ----
        [TestMethod]
        public void Costume_WithZeroAvailable_IsNotAvailable()
        {
            // ARRANGE + ACT: all 2 costumes are out (0 available).
            Costume costume = new Costume(
                "GA007", "Court Dress", "Royal", "Tudor", "L",
                1100m, 2, 0
            );

            // ASSERT: IsAvailable should be false when availableQuantity is 0.
            Assert.IsFalse(costume.IsAvailable);
        }


    }
}