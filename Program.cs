using System.Collections;
using System.Text;
using System;

namespace ExampleEvent
{
    delegate void AccountStateHandler(object sender,AccountEventArgs e);
    class AccountEventArgs
    {
        public string Message { get; }
        public int Sum { get; }
        public AccountEventArgs(string message, int sum)
        {
            Message = message;
            Sum = sum;
        }
    }
    class Account
    {
        int _sum;
        public event AccountStateHandler Added; //Past participle
        public event AccountStateHandler Adding; // ing - form
        public event AccountStateHandler Withdrawn; // Past participle
        public Account() { }
        public Account(int sum) => _sum = sum;

        public void Put(int sum)
        {
            if (Adding != null) Adding(this,
                new AccountEventArgs($"На счет добавляется {sum}", sum));
            _sum += sum;
            if (Added != null) Added(this, new AccountEventArgs($"На счет пришло {sum}", sum));
        }
            public void WithDraw(int sum)
            {
                if (_sum >= sum)
                {
                    _sum -= sum;
                    if (Withdrawn != null) Withdrawn(this, new AccountEventArgs($"Со счета снято { sum}", sum));
                }
                else
                {
                    if (Withdrawn != null) Withdrawn(this, new AccountEventArgs("На счете недостаточно средств", 0));
                }
            }
        class Program
        {
            static void Main(string[] args)
            {
                Account account = new Account();
                account.Added += new AccountStateHandler(Display);
                account.Withdrawn += Display;
                account.Put(150);
                account.WithDraw(100);
                account.Withdrawn -= Display;
                account.WithDraw(150);
            }

            static void Display(object sender, AccountEventArgs e)
            {
                Console.WriteLine($"Сумма транзакции: {e.Sum}");
                Console.WriteLine(e.Message);
            }
        }
    }
}

