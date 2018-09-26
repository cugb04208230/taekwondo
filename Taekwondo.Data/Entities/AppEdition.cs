using Common.Data;

namespace Taekwondo.Data.Entities
{
    public class AppEdition:Entity
    {
	    public string Os { get; set; }

	    public string Edition { get; set; }

		public string Path { set; get; }

		public bool IsMandatory { set; get; }

		public string Content { set; get; }

	}
}
