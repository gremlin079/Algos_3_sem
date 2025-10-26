using System;

namespace ClassLibrary
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommentAttribute : Attribute
    {
        public string Comment { get; }

        public CommentAttribute(string comment)
        {
            Comment = comment;
        }
    }
}
