using System.ComponentModel.DataAnnotations;

namespace proj
{
	public class Product : basemodel
	{
		[Required]
		[MinLength(2)]
		public string Name{get;set;}

		[Required]
		public double Price{get;set;}
	}
}
