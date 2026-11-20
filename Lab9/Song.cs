class Song
{
    private string name;
    private string author;
    private Song? previous;

    public Song() : this("Без названия", "Неизвестный автор", null) { }
    public Song(string name, string author) : this(name, author, null) { }
    public Song(string name, string author, Song? previous)
    {
        this.name = name;
        this.author = author;
        this.previous = previous;
    }
    public string Title() { return name + " — " + author; }
    public void Print()
    {
        Console.WriteLine(Title() + (previous == null ? "" : "; предыдущая: " + previous.Title()));
    }
    public override bool Equals(object? obj)
    {
        Song? other = obj as Song;
        return other != null && name == other.name && author == other.author;
    }
    public override int GetHashCode() { return HashCode.Combine(name, author); }
}
