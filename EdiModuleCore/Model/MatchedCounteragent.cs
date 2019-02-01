using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiModuleCore.Model
{
    using Bridge1C.DomainEntities;

    public class MatchedCounteragent
    {
        public Counteragent InnerCounteragent { get; set; }
        public ExCounteragent ExCounteragent { get; set; }

		public override string ToString()
		{
			return string.Format("{0}", InnerCounteragent?.Name);
		}

		public override bool Equals(object obj)
		{
			if(obj is MatchedCounteragent other)
			{
				try
				{
					return this.InnerCounteragent.Equals(other.InnerCounteragent) &&
						this.ExCounteragent.Equals(other.ExCounteragent);
				}
				catch(NullReferenceException)
				{}
			}

			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
