using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Models
{
	public class user
	{
		[Key]
		[MaxLength(50)]
		public string ?username { get; set; }

		[Required]
		[MaxLength(50)]
		public string ?password { get; set; }
	}
}
