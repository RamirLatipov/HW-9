enum ProjectStatus { Project, Active, Closed }
enum TaskStatus { Assigned, InProgress, Review, Done }

class Person
{
    public string Name { get; }
    public Person(string name) { Name = name; }
    public override string ToString() { return Name; }
}

class Report
{
    public string Text { get; private set; }
    public DateTime Date { get; private set; }
    public Person Author { get; private set; }
    public Report(string text, Person author) { Text = text; Author = author; Date = DateTime.Now; }
    public void Update(string text, Person author) { Text = text; Author = author; Date = DateTime.Now; }
}

class WorkTask
{
    public string Description { get; }
    public DateTime Deadline { get; }
    public Person Initiator { get; }
    public Person? Executor { get; private set; }
    public TaskStatus Status { get; private set; } = TaskStatus.Assigned;
    public Report? Report { get; private set; }
    private readonly Project project;

    public WorkTask(Project project, string description, DateTime deadline, Person initiator, Person executor)
    {
        this.project = project; Description = description; Deadline = deadline;
        Initiator = initiator; Executor = executor;
    }
    private void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
    public void Start(Person person)
    {
        Require(project.Status == ProjectStatus.Active && Status == TaskStatus.Assigned && Executor == person, "Задачу может взять назначенный исполнитель во время исполнения проекта.");
        Status = TaskStatus.InProgress;
    }
    public void Delegate(Person person, Person next)
    {
        Require(project.Status == ProjectStatus.Active && Status == TaskStatus.Assigned && Executor == person, "Делегировать может назначенный исполнитель.");
        Executor = next;
    }
    public void Reject(Person person)
    {
        Require(project.Status == ProjectStatus.Active && Status == TaskStatus.Assigned && Executor == person, "Отклонить задачу может назначенный исполнитель.");
        Executor = null;
    }
    public void Assign(Person person, Person next)
    {
        Require(project.Status == ProjectStatus.Active && Initiator == person && Status == TaskStatus.Assigned && Executor == null, "Повторно назначить отклонённую задачу может инициатор.");
        Executor = next;
    }
    public void Submit(Person person, string text)
    {
        Require(project.Status == ProjectStatus.Active && Executor == person && Status == TaskStatus.InProgress && !string.IsNullOrWhiteSpace(text), "Отчитаться может исполнитель задачи в работе; текст не должен быть пустым.");
        if (Report == null) Report = new Report(text, person);
        else Report.Update(text, person); // После доработки изменяем тот же отчёт.
        Status = TaskStatus.Review;
    }
    public void Review(Person person, bool approved)
    {
        Require(project.Status == ProjectStatus.Active && Initiator == person && Status == TaskStatus.Review && Report != null, "Отчёт проверяет инициатор задачи.");
        Status = approved ? TaskStatus.Done : TaskStatus.InProgress;
    }
    public void Print()
    {
        string status = Status switch { TaskStatus.Assigned => "Назначена", TaskStatus.InProgress => "В работе", TaskStatus.Review => "На проверке", _ => "Выполнена" };
        Console.WriteLine($"«{Description}»: {status}; исполнитель: {Executor?.Name ?? "не назначен"}; срок: {Deadline:dd.MM.yyyy}");
        if (Report != null) Console.WriteLine($"  Отчёт: {Report.Text}; {Report.Date:g}; автор: {Report.Author}");
    }
}

class Project
{
    public string Description { get; }
    public DateTime Deadline { get; }
    public Person Customer { get; }
    public Person Lead { get; }
    public ProjectStatus Status { get; private set; } = ProjectStatus.Project;
    private readonly List<WorkTask> tasks = new List<WorkTask>();
    public IReadOnlyList<WorkTask> Tasks { get { return tasks.AsReadOnly(); } }
    public Project(string description, DateTime deadline, Person customer, Person lead)
    { Description = description; Deadline = deadline; Customer = customer; Lead = lead; }
    public WorkTask CreateTask(Person person, string description, DateTime deadline, Person executor)
    {
        if (person != Lead || Status != ProjectStatus.Project) throw new InvalidOperationException("Создавать задачи может только тимлид, пока проект в статусе «Проект».");
        if (deadline > Deadline) throw new ArgumentException("Срок задачи позже срока проекта.");
        WorkTask task = new WorkTask(this, description, deadline, person, executor);
        tasks.Add(task);
        return task;
    }
    public void Start(Person person)
    {
        if (person != Lead || Status != ProjectStatus.Project || tasks.Count == 0 || tasks.Any(t => t.Executor == null)) throw new InvalidOperationException("Тимлид запускает проект с назначенными задачами.");
        Status = ProjectStatus.Active;
    }
    public void DeleteRejected(Person person, WorkTask task)
    {
        if (person != Lead || Status != ProjectStatus.Active || !tasks.Contains(task) || task.Executor != null || task.Status != TaskStatus.Assigned) throw new InvalidOperationException("Удалить можно только отклонённую задачу своего проекта.");
        tasks.Remove(task);
    }
    public void Close(Person person)
    {
        if (person != Lead || Status != ProjectStatus.Active || tasks.Count == 0 || tasks.Any(t => t.Status != TaskStatus.Done)) throw new InvalidOperationException("Закрыть проект можно после выполнения всех задач.");
        Status = ProjectStatus.Closed;
    }
}
