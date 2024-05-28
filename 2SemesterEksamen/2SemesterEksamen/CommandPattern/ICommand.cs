namespace CommandPattern
{
    /// <summary>
    /// The interface for the command pattern
    /// </summary>
    internal interface ICommand
    {
        void Execute();
        void Undo();
    }
}
