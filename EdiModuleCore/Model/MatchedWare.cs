namespace EdiModuleCore.Model
{
    using Bridge1C.DomainEntities.Spr;

	/// <summary>
	/// Сопоставленная сущность номенклатуры.
	/// </summary>
    public class MatchedWare
    {
		/// <summary>
		/// Номенклатура из базы.
		/// </summary>
        public Ware InnerWare { get; set; }
		/// <summary>
		/// Номенклатура из накладной.
		/// </summary>
        public ExWare ExWare { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj is MatchedWare ware))
            {
                bool result = this.ExWare.Equals(ware.ExWare);

                if (this.InnerWare != null && ware.InnerWare != null)
                    result = result && this.InnerWare.Equals(ware.InnerWare);

                return result;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
