namespace Connector.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// Идентификатор команды
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Выполняемый метод
        /// </summary>
        void Invoke(string[] args);
    }
}