namespace Framework.Interfaces.Adapters;

public interface IWebElementAdapter
{
    void Click();
    void SendKeys(string text);
    string GetText();
    string Text { get; }
}
