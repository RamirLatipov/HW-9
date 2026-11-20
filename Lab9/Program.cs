using System.Text;
class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Упражнение 9.3. Сохранение проводок."); using BankAccount account = new BankAccount(1000); account.Deposit(250); account.Withdraw(100); account.Print(); account.Dispose();
    }
}
