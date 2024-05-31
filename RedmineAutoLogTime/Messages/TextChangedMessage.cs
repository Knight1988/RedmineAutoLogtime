namespace RedmineAutoLogTime.Messages;

public class TextChangedMessage
{
    public string Id { get; }
    public string NewValue { get; }

    public TextChangedMessage(string id, string newValue)
    {
        Id = id;
        NewValue = newValue;
    }
}