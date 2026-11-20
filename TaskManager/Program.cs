using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Task Manager. Демонстрация проекта и разовых задач; ввод не требуется.\n");
        string[] names = { "Алексей", "Дина", "Марат", "Ильдар", "Антон", "Ляйсан", "Илья", "Витя", "Женя", "Сергей" };
        List<Person> team = new List<Person>();
        foreach (string name in names) team.Add(new Person(name));
        Person lead = team[0];
        Person customer = new Person("Заказчик");
        DateTime deadline = DateTime.Today.AddDays(30);
        Project project = new Project("Создать приложение для компании", deadline, customer, lead);
        Console.WriteLine($"Проект: {project.Description}; срок: {project.Deadline:dd.MM.yyyy}; заказчик: {project.Customer}; тимлид: {project.Lead}; статус: Проект.");
        for (int i = 0; i < team.Count; i++)
        {
            WorkTask task = project.CreateTask(lead, $"Задача {i + 1}: этап разработки", deadline.AddDays(-1), team[i]);
            task.Print();
        }
        project.Start(lead);
        Console.WriteLine("\nПроект переведён в статус «Исполнение».");
        WorkTask delegated = project.Tasks[1];
        delegated.Delegate(team[1], team[2]);
        Console.WriteLine($"{team[1]} делегирует задачу сотруднику {team[2]}.");
        WorkTask rejected = project.Tasks[2];
        rejected.Reject(team[2]);
        Console.WriteLine($"{team[2]} отклоняет задачу; исполнитель не назначен.");
        rejected.Assign(lead, team[1]);
        Console.WriteLine($"{lead} повторно назначает её сотруднику {team[1]}.\n");
        foreach (WorkTask task in project.Tasks)
        {
            Person executor = task.Executor!;
            task.Start(executor);
            task.Submit(executor, "Работа выполнена, результат подготовлен.");
            if (task == delegated)
            {
                task.Review(lead, false);
                Console.WriteLine("Отчёт возвращён на доработку, затем дополнен.");
                task.Submit(executor, "Недочёты исправлены, результат проверен.");
            }
            task.Review(lead, true);
            task.Print();
        }
        project.Close(lead);
        Console.WriteLine("\nВсе задачи выполнены. Статус проекта: Закрыт.");
    }
}
