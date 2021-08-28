namespace EventHandling
{
    public class EventArgs
    {

        /// < summary > event type </summary >
        public readonly string type;
        /// < summary > event parameters </summary>
        public readonly object[] args;

        public EventArgs(string type)
        {
            this.type = type;
        }

        public EventArgs(string type, params object[] args)
        {
            this.type = type;
            this.args = args;
        }

    }
}
