using System.ComponentModel.DataAnnotations;

namespace proj
{
	public class User : basemodel
	{
		[Display(Name="First Name")]
		[Required]
		[MinLength(3)]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="No numbers or special characters allowed in name")]
		public string FirstName{get;set;}
		[Display(Name="Last Name")]
		[Required]
		[MinLength(3)]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="No numbers or special characters allowed in name")]
		public string LastName{get;set;}
		[Display(Name="Disposable Email")]
		[Required]
		[EmailAddress]
		public string Email{get;set;}
		[Display(Name="Secret pass code")]
		[Required]
		[MinLength(8)]
		[DataType(DataType.Password)]
		public string Password{get;set;}
		[Display(Name="Confirm pass code")]
		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage="Passwords must match")]
		public string PassConfirm{get;set;}
	}
}
