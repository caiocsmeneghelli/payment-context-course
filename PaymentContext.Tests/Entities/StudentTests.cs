using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentTests
    {
        private readonly Student _student;
        private readonly Subscription _subscription;

        private readonly Name _name;
        private readonly Document _document;
        private readonly Address _address;
        private readonly Email _email;
        private readonly Payment _payment;

        public StudentTests()
        {
            _name = new Name("Bruce", "Wayne");
            _document = new Document("01269035061", EDocumentType.CPF);
            _address = new Address("Rua 1", "1234", "Bairro Legal", "Gotham", "São Paulo", "Brasil", "221143");
            _email = new Email("batman@wayne.com");
            _student = new Student(_name, _document, _email);
            _subscription = new Subscription(null);
            _payment = new PayPalPayment("123456789",
                        DateTime.Now.AddMinutes(2), DateTime.Now.AddDays(5), 10, 10,
                        "Wayne Corp", _document, _address, _email);
        }
        [TestMethod]
        public void ShouldReturnErrorWhenHadActiveSubscription()
        {
            _subscription.AddPayment(_payment);
            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);
        }
        [TestMethod]
        public void ShouldReturnErrorWhenSubscriptionHasNoPayment()
        {
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);
        }
        [TestMethod]
        public void ShouldReturnSuccessWhenHadNoActiveSubscription()
        {
            _subscription.AddPayment(_payment);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Valid);
        }
    }
}
