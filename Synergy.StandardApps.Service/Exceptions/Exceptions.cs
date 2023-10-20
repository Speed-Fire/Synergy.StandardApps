using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Service.Exceptions
{
	public class InvalidFormException : Exception { }
	public class InvalidIdException : Exception { }
    public class NameIsAlreadyTakenExceptionException : Exception { }
    public class CalendarDateIsAlreadyTakenException : Exception { }
	public class AlarmTimeIsAlreadyTakenException : Exception { }
}
