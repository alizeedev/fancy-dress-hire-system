using Microsoft.VisualStudio.TestTools.UnitTesting;
using FancyDressHireSystem.Core.Services;

namespace FancyDressHireSystem.Tests
{
    [TestClass]
    public class PasswordHasherTests
    {
        // ---- Same password + same salt should give the same hash ----
        [TestMethod]
        public void Hash_SamePasswordAndSalt_GivesSameHash()
        {
            string salt = PasswordHasher.GenerateSalt();

            string hash1 = PasswordHasher.Hash("MyPassword123", salt);
            string hash2 = PasswordHasher.Hash("MyPassword123", salt);

            // Hashing the same thing twice must always match.
            Assert.AreEqual(hash1, hash2);
        }

        // ---- The hash should NOT be the plain password ----
        [TestMethod]
        public void Hash_Result_IsNotThePlainPassword()
        {
            string salt = PasswordHasher.GenerateSalt();
            string hash = PasswordHasher.Hash("MyPassword123", salt);

            // The stored hash must be scrambled, never the real password.
            Assert.AreNotEqual("MyPassword123", hash);
        }

        // ---- Verify with the CORRECT password should return true ----
        [TestMethod]
        public void Verify_WithCorrectPassword_ReturnsTrue()
        {
            string salt = PasswordHasher.GenerateSalt();
            string storedHash = PasswordHasher.Hash("MyPassword123", salt);

            bool result = PasswordHasher.Verify("MyPassword123", storedHash, salt);

            Assert.IsTrue(result);
        }

        // ---- Verify with the WRONG password should return false ----
        [TestMethod]
        public void Verify_WithWrongPassword_ReturnsFalse()
        {
            string salt = PasswordHasher.GenerateSalt();
            string storedHash = PasswordHasher.Hash("MyPassword123", salt);

            bool result = PasswordHasher.Verify("WrongPassword", storedHash, salt);

            Assert.IsFalse(result);
        }

        // ---- Verify with empty inputs should return false (guard clause) ----
        [TestMethod]
        public void Verify_WithEmptyPassword_ReturnsFalse()
        {
            string salt = PasswordHasher.GenerateSalt();
            string storedHash = PasswordHasher.Hash("MyPassword123", salt);

            bool result = PasswordHasher.Verify("", storedHash, salt);

            Assert.IsFalse(result);
        }

        // ---- Two generated salts should be different (randomness) ----
        [TestMethod]
        public void GenerateSalt_CalledTwice_GivesDifferentSalts()
        {
            string salt1 = PasswordHasher.GenerateSalt();
            string salt2 = PasswordHasher.GenerateSalt();

            // Random salts should (practically) never be the same.
            Assert.AreNotEqual(salt1, salt2);
        }
    }
}