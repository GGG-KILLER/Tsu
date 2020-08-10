namespace GUtils.CLI.Commands
{
    /// <summary>
    /// A command that is actually a verb.
    /// </summary>
    public interface IVerbCommand
    {
        /// <summary>
        /// The command manager that this verb redirects its commands to.
        /// </summary>
        public BaseCommandManager CommandManager { get; }
    }
}