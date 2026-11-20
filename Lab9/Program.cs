using System.Text;
class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Упражнение 9.2. Банковские операции."); using BankAccount account = new BankAccount(1000); account.Deposit(250); account.Withdraw(100); Console.WriteLine("Снятие 2000: " + (account.Withdraw(2000) ? "выполнено" : "недостаточно денег")); account.Print();
    }
}
