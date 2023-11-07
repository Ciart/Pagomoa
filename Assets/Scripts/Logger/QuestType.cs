using System.Collections.Generic;

namespace Logger
{
    public class QuestType<T>
    {
        public string summary;
        public T value;

        public QuestType(string summary, T value)
        {
            this.summary = summary;
            this.value = value;
        }
    }
}