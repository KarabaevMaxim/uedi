namespace DAL.DomainEntities.Spr
{
    public class Unit
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string International { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj is Unit unit))
                return  this.Code == unit.Code &&
                        this.Name == unit.Name &&
                        this.FullName == unit.FullName &&
                        this.International == unit.International;

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
