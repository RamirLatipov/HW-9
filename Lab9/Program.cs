using System.Text;
class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Упражнение 9.1. Конструкторы счёта."); using BankAccount a = new BankAccount(); using BankAccount b = new BankAccount(1000); using BankAccount c = new BankAccount(AccountType.Savings); using BankAccount d = new BankAccount(500, AccountType.Savings); a.Print(); b.Print(); c.Print(); d.Print();
    }
}
