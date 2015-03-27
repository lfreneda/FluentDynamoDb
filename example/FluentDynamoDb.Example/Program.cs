using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentDynamoDb.Configuration;
using FluentDynamoDb.Converters;
using FluentDynamoDb.Mappers;

namespace FluentDynamoDb.Example
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Contact Contact { get; set; }

        private readonly IList<Account> _accounts = new List<Account>();
        public IEnumerable<Account> Accounts { get { return _accounts; } }

        public void AddAccount(Account account)
        {
            _accounts.Add(account);
        }

        public override string ToString()
        {
            var accountsSb = new StringBuilder();
            foreach (var account in _accounts)
            {
                accountsSb.Append(account);
            }

            return string.Format("Id: {0}, Name: {1}, BirthDate: {2}, Gender: {3}, Contact: {4}, Accounts: {5}", Id, Name, BirthDate, Gender, Contact, accountsSb);
        }
    }

    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }

        public override string ToString()
        {
            return string.Format("Username: {0}, Password: {1}, Balance: {2}", Username, Password, Balance);
        }
    }

    public class Contact
    {
        public string Email { get; set; }
        public int PhoneNumber { get; set; }

        public override string ToString()
        {
            return string.Format("Email: {0}, PhoneNumber: {1}", Email, PhoneNumber);
        }
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            TableName("Users");

            Map(u => u.Id);
            Map(u => u.Name);
            Map(u => u.BirthDate);
            Map(u => u.Gender).With(new DynamoDbConverterEnum<Gender>());

            References(u => u.Contact);

            HasMany(u => u.Accounts).With(AccessStrategy.CamelCaseUnderscoreName);
        }
    }

    public class ContactMap : ClassMap<Contact>
    {
        public ContactMap()
        {
            Map(c => c.Email);
            Map(c => c.PhoneNumber);
        }
    }

    public class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Map(c => c.Username);
            Map(c => c.Password);
            Map(c => c.Balance);
        }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            /*
             * You must set your DynamoDb configuration @ app.config
             * 
             *   <appSettings>
                    <add key="AWSProfileName" value="" />
                    <add key="AWSRegion" value="us-east-1" />
                    <add key="AWSAccessKey" value=""/>
                    <add key="AWSSecretKey" value=""/>
                  </appSettings>
             * 
             */

            FluentDynamoDbConfiguration.Configure(Assembly.GetExecutingAssembly());

            var userStore = new DynamoDbStore<User>();

            var user = new User
            {
                Id = new Guid("1a9f6ee7-d0bf-47ab-a6f6-6225ebe713d8"),
                Name = "Luiz Freneda",
                BirthDate = new DateTime(1988, 05, 15),
                Gender = Gender.Male,
                Contact = new Contact
                {
                    Email = "lfreneda@gmail.com",
                    PhoneNumber = 963427000
                }
            };

            user.AddAccount(new Account { Balance = 9999.99m, Password = "password", Username = "lfreneda" });
            user.AddAccount(new Account { Balance = 123.45m, Password = "password", Username = "lfrened4" });

            userStore.PutItem(user);

            var savedUser = userStore.GetItem(new Guid("1a9f6ee7-d0bf-47ab-a6f6-6225ebe713d8"));
            Console.WriteLine(savedUser);

            savedUser.Name = "Luiz Freneda Perez Junior";
            userStore.UpdateItem(savedUser);

            savedUser = userStore.GetItem(new Guid("1a9f6ee7-d0bf-47ab-a6f6-6225ebe713d8"));
            Console.WriteLine(savedUser);

            userStore.DeleteItem(new Guid("1a9f6ee7-d0bf-47ab-a6f6-6225ebe713d8"));

            Console.Read();
        }
    }
}
