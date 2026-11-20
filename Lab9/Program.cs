using System.Text;
class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Упражнение 9.1. Четыре конструктора банковского счёта:");
        using BankAccount first = new BankAccount();
        using BankAccount second = new BankAccount(1000);
        using BankAccount third = new BankAccount(AccountType.Savings);
        using BankAccount fourth = new BankAccount(500, AccountType.Savings);
        first.Print(); second.Print(); third.Print(); fourth.Print();

        Console.WriteLine("\nУпражнение 9.2. Пополнение и снятие записываются в очередь:");
        second.Deposit(250);
        second.Withdraw(100);
        Console.WriteLine("Снятие 2000: " + (second.Withdraw(2000) ? "выполнено" : "недостаточно денег"));
        fourth.TransferFrom(second, 200);
        second.Print(); fourth.Print();

        Console.WriteLine("\nУпражнение 9.3. При выходе из программы Dispose сохранит проводки.");
        Console.WriteLine("\nДомашнее задание 9.1. Конструкторы песен:");
        Song mySong = new Song();
        Song song1 = new Song("Кукушка", "Кино");
        Song song2 = new Song("Группа крови", "Кино", song1);
        mySong.Print(); song1.Print(); song2.Print();
        Console.WriteLine("Без конструктора Song() строка new Song() вызвала бы ошибку компиляции.");
    }
}
