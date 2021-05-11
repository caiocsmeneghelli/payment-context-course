using Flunt.Validations;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public Address(string street, string number, string neighborhood, string city, string estate, string country, string zipCode)
        {
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            Estate = estate;
            Country = country;
            ZipCode = zipCode;

            AddNotifications(new Contract()
            .Requires()
            .HasMinLen(Street, 3, "Address.Street", "A rua deve conter pelo menos 3 caracteres.")
            .HasMinLen(Neighborhood, 3, "Address.Neighborhood", "O bairro deve conter pelo menos 3 caracteres.")
            .HasMinLen(City, 3, "Address.City", "A cidade deve conter pelo menos 3 caracteres.")
            .HasMinLen(Estate, 3, "Address.Estate", "O estado deve conter pelo menos 3 caracteres.")
            .HasMinLen(ZipCode, 3, "Address.ZipCode", "O cep deve conter pelo menos 3 caracteres."));
        }

        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string Estate { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
    }
}