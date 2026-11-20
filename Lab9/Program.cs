using System.Text;
class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Домашнее задание 9.1. Конструкторы песен."); Song mySong = new Song(); Song first = new Song("Кукушка", "Кино"); Song second = new Song("Группа крови", "Кино", first); mySong.Print(); first.Print(); second.Print(); Console.WriteLine("Без конструктора Song() вызов new Song() вызвал бы ошибку компиляции.");
    }
}
