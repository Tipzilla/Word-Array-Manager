public class Node
{
    public string Word { get; private set; }
    public int Length { get; private set; }

    public Node(string word)
    {
        Word = word;
        Length = word.Length;
    }

    public override string ToString()
    {
        return $"Word: {Word}, Length: {Length}";
    }
}
