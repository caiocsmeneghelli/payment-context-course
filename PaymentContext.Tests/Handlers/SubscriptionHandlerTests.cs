using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        // red, green, refactor
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();


            command.FirstName = "Caio Cesar";
            command.LastName = "Meneghelli";
            command.DocumentNumber = "11111111112";
            command.Email = "hello1@outlook.com";

            command.BarCode = "123456789";
            command.BoletoNumber = "1232233222";

            command.PaymentNumber = "123131313";
            command.PaidDate = DateTime.Now.AddDays(5);
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "Wayson";
            command.PayerDocument = "11111111112";
            command.PayerDocumentType = Domain.Enums.EDocumentType.CPF;
            command.PayerEmail = "hello1@outlook.com";

            command.Street = "asdada";
            command.AddressNumber = "dadada";
            command.Neighborhood = "dadada";
            command.City = "adadad";
            command.State = "adadad";
            command.Country = "dadaada";
            command.ZipCode = "dadadada";

            handler.Handle(command);
            Assert.AreEqual(true, handler.Valid);
        }
    }
}