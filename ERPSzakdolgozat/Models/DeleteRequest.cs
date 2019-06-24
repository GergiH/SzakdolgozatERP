using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class DeleteRequest : BaseProperties
	{
		[Required]
		[ForeignKey("AppUser")]
		public int AppUserId { get; set; }
		public bool IsFulfilled { get; set; }
	}
}