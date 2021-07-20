namespace LoxLangInCSharp
{
    public class Klass
    {
        private readonly string name;

        public Klass(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}