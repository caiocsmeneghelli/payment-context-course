using System;
using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : Notifiable, IHandler<CreateBoletoSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possivel realizar sua assinatura.");
            }

            // Verificar se documento ja esta cadastrado.
            if (_repository.DocumentExists(command.DocumentNumber))
                AddNotification("Document", "Este CPF já esta em uso");

            // Verificar se email ja esta cadastrado
            if (_repository.EmailExists(command.Email))
                AddNotification("Email", "Este email ja esta em uso");

            // Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.DocumentNumber, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.AddressNumber, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);


            // Gerar as Entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(command.BarCode, command.BoletoNumber,
             command.PaidDate, command.ExpireDate, command.Total, command.TotalPaid,
             command.Payer, new Document(command.PayerDocument, command.PayerDocumentType), address, email);

            // Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            // Aplicar as validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            // Salvar as informações
            _repository.CreateSubscription(student);

            // Enviar email de boas vindas.
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Boas Vindas", $"Welcome {student.Name.ToString()}, sua assinatura foi criada!");

            // Retornar email de boas vindas.

            // Retornar informações.

            return new CommandResult(true, "Assinatura realizada com sucesso.");
        }
    }
}