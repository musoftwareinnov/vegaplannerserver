using System;
using AutoMapper;

namespace vega.Mapping
{

    public class DateTimeNullConverter : ITypeConverter<DateTime, string>
    {    public string Convert(DateTime input, string output, ResolutionContext context)
        {

            if (input != null)
                return "YOOOOOO!";
            else
                return "NULLER!";
        }
    }

    // public class NullableDateTimeConverter : ITypeConverter<DateTime?, DateTime?>
    // {
    //     protected override DateTime? ConvertCore(DateTime? source)
    //     {
    //         return source;
    //     }
    // }
}