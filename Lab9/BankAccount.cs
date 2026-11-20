using System.Collections;

enum AccountType { Current, Savings }

class BankTransaction
{
    public readonly DateTime Date;
    public readonly decimal Amount;

    public BankTransaction(decimal amount)
    {
        Date = DateTime.Now;
        Amount = amount;
    }
}

class BankAccount : IDisposable
{
    private static int nextNumber = 1;
    private readonly int number;
    private readonly AccountType type;
    private decimal balance;
    private readonly Queue transactions = new Queue();
    private bool disposed;

    // Все конструкторы вызывают общий конструктор, который создаёт номер.
    public BankAccount() : this(0, AccountType.Current) { }
    public BankAccount(decimal balance) : this(balance, AccountType.Current) { }
    public BankAccount(AccountType type) : this(0, type) { }
    public BankAccount(decimal balance, AccountType type)
    {
        if (balance < 0) throw new ArgumentException("Начальный баланс не может быть отрицательным.");
        number = GenerateNumber();
        this.balance = balance;
        this.type = type;
    }

    private static int GenerateNumber() { return nextNumber++; }
    public decimal GetBalance() { return balance; }
    public void Print()
    {
        string name = type == AccountType.Current ? "текущий" : "сберегательный";
        Console.WriteLine($"Счёт №{number}, тип: {name}, баланс: {balance}");
    }
    public bool Deposit(decimal amount)
    {
        if (disposed || amount <= 0) return false;
        balance += amount;
        transactions.Enqueue(new BankTransaction(amount));
        return true;
    }
    public bool Withdraw(decimal amount)
    {
        if (disposed || amount <= 0 || amount > balance) return false;
        balance -= amount;
        transactions.Enqueue(new BankTransaction(-amount));
        return true;
    }
    public bool TransferFrom(BankAccount source, decimal amount)
    {
        if (disposed || source == null || source == this || !source.Withdraw(amount)) return false;
        return Deposit(amount);
    }
    public void Dispose()
    {
        if (disposed) return;
        Directory.CreateDirectory("transactions");
        string path = Path.Combine("transactions", $"account-{number}.txt");
        // После перезапуска программы номера начинаются заново, поэтому записи дописываются.
        using (StreamWriter writer = new StreamWriter(path, true))
        {
            foreach (BankTransaction transaction in transactions)
                writer.WriteLine($"{transaction.Date:yyyy-MM-dd HH:mm:ss}; {transaction.Amount:+0.00;-0.00;0.00}");
        }
        transactions.Clear();
        disposed = true;
        GC.SuppressFinalize(this);
        Console.WriteLine($"Проводки счёта №{number} записаны: {Path.GetFullPath(path)}");
    }
}
