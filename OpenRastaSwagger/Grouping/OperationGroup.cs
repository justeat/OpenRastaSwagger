namespace OpenRastaSwagger.Grouping
{
    public class OperationGroup
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this)) return true;

            var other = obj as OperationGroup;
            if (other == null) return false;

            return Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}