namespace Bridge1C.DomainEntities
{
    public class WareExCode
    {
        public Counteragent Counteragent { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Counteragent, this.Value);
        }

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj is WareExCode exCode))
                return  this.Counteragent.Equals(exCode.Counteragent) &&
                        this.Value == exCode.Value;

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
