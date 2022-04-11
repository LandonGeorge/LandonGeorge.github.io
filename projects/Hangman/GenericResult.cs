using System.Collections.Generic;
using System.Linq;

namespace hangman.Controllers
{
	public class GenericResult
	{
		public bool IsValid { get { return !ErrorMessages.Any(); } }
		public List<string> ErrorMessages;
		public List<string> SuccessMessages;
		public List<string> WarningMessages;
		public string RedirectUrl { get; set; }

		public GenericResult()
		{
			ErrorMessages = new List<string>();

		}
	}

	public class GenericResult<T> : GenericResult
	{
		public T Data { get; set; }
	}
}