namespace LostObjects.Common.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Objectt
	{
		[Key]
		public int ObjectId { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		[Required]
		[StringLength(20)]
		public string Type { get; set; }

		public string PhoneContact { get; set; }

		[Display(Name = "Publish On")]
		[DataType(DataType.Date)]
		public DateTime PublishOn { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		[Required]
		[StringLength(150)]
		public string Description { get; set; }

		[Required]
		[StringLength(50)]
		public string Location { get; set; }

		[Display(Name = "Image")]
		public string ImagePath { get; set; }

		[Display(Name = "Is delivered?")]
		public bool IsDelivered { get; set; }

		[NotMapped]
		public byte[] ImageArray { get; set; }

		public string ImageFullPath
		{
			get
			{
				if (string.IsNullOrEmpty(this.ImagePath))
				{
					return "NoObject";
				}

				return $"https://lostobjectsapi.azurewebsites.net/{this.ImagePath.Substring(1)}";
			}
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
