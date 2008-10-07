using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Toolbox
{
	public static class DateTimeExtensions
	{
		//
		// TODO: Standardize if/then/else braces
		//
		/// <summary>
		/// The client's time zone offset, which has to be set for any ToClientTime calls to be correct
		/// </summary>
		public static double ClientTimezoneOffset { get; set; }
		/// <summary>
		/// The application's time zone offset, which has to be set for any ToApplicationTime calls to be correct
		/// </summary>
		public static double ApplicationTimezoneOffset { get; set; }
		/// <summary>
		/// The local time zone offset
		/// </summary>
		public static double ServerTimezoneOffset
		{
			get { return TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours; }
		}
		/// <summary>
		/// Stores a set of DateTimeFormats for the entire application
		/// </summary>
		public static Dictionary<string, string> DateTimeFormats
		{
			get { return m_DateTimeFormats; }
		}
		static Dictionary<string, string> m_DateTimeFormats;

		static DateTimeExtensions()
		{
			m_DateTimeFormats = new Dictionary<string, string>();
		}

		/// <summary>
		/// Converts a server time to the client's time
		/// </summary>
		public static DateTime? GetClientTime(DateTime? dt)
		{
			if (dt.HasValue)
			{
				if (dt.Value.Kind == DateTimeKind.Local)
				{
					return dt.Value.ToUniversalTime().AddHours(ClientTimezoneOffset);
				}
				else // assume UTC
				{
					return dt.Value.AddHours(ClientTimezoneOffset);
				}
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Converts the server time to the timezone set for the application, not neccessarily the client time
		/// </summary>
		public static DateTime? GetApplicationTime(DateTime? dt)
		{
			if (dt.HasValue)
			{
				if (dt.Value.Kind == DateTimeKind.Local)
				{
					return dt.Value.ToUniversalTime().AddHours(ApplicationTimezoneOffset);
				}
				else // assume UTC
				{
					return dt.Value.AddHours(ClientTimezoneOffset);
				}
			}
			else
			{
				return null;
			}
		}

		//
		// TODO: Cleanup this math if possible
		//
		/// <summary>
		/// Converts a client entered time to the actual time the client meant, not the server time.
		/// </summary>
		public static DateTime? GetServerTime(DateTime? dt)
		{
			if (dt.HasValue)
			{
				return dt.Value.AddHours(-1 * (ClientTimezoneOffset - ServerTimezoneOffset));
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Returns a DateTime adjusted for the client's time zone
		/// </summary>
		public static DateTime ToClientTime(this DateTime dt)
		{
			return GetClientTime(dt).Value;
		}
		/// <summary>
		/// Returns a DateTime adjusted for the client's time zone
		/// </summary>
		public static DateTime? ToClientTime(this DateTime? dt)
		{
			if (dt.HasValue)
				return GetClientTime((DateTime)dt);
			else
				return dt;
		}

		/// <summary>
		/// Returns the datetime a client intended to enter, if they had entered it adjusted for the client to server time zone difference.
		/// </summary>
		public static DateTime ToServerTime(this DateTime dt)
		{
			return GetServerTime(dt).Value;
		}
		/// <summary>
		/// Returns the datetime a client intended to enter, if they had entered it adjusted for the client to server time zone difference.
		/// </summary>
		public static DateTime? ToServerTime(this DateTime? dt)
		{
			if (dt.HasValue)
				return GetServerTime(dt);
			else
				return null;
		}

		/// <summary>
		/// Returns a DateTime adjusted for the applications's time zone
		/// </summary>
		public static DateTime ToApplicationTime(this DateTime dt)
		{
			return GetApplicationTime(dt).Value;
		}
		/// <summary>
		/// Returns a DateTime adjusted for the applications's time zone
		/// </summary>
		public static DateTime? ToApplicationTime(this DateTime? dt)
		{
			if (dt.HasValue)
				return GetApplicationTime(dt);
			else
				return dt;
		}

		/// <summary>
		/// Formats the DateTime using the value in DateTimeFormats that matches formatKey
		/// </summary>
		public static string ToFormat(this DateTime dt, string formatKey)
		{
			if (DateTimeFormats.ContainsKey(formatKey))
			{
				return dt.ToString(DateTimeFormats[formatKey]);
			}
			else
			{
				return dt.ToString(formatKey);
			}
		}
		/// <summary>
		/// Formats the DateTime using the value in DateTimeFormats that matches formatKey
		/// </summary>
		public static string ToFormat(this DateTime? dt, string formatKey)
		{
			if (dt.HasValue)
			{
				return ToFormat(dt.Value, formatKey);
			}
			else
			{
				return string.Empty;
			}
		}
	}
}