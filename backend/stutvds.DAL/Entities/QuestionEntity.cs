using System;
using StopStatAuth_6_0.Entities.Base;

namespace StopStatAuth_6_0.Entities
{
	public class QuestionEntity : Entity
	{
		public string Question { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsReviewed { get; set; }
		public string Culture { get; set; }
	}
}
