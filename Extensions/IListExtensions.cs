using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using vega.Core.Models;
using vega.Core.Models.States;

namespace vega.Extensions
{
    public static class IListExtensions
    {
        public static List<ProjectGeneratorSequence> ApplySequenceSorting<T>(this List<ProjectGeneratorSequence> query) 
        {
            return query.OrderBy(g => g.SeqId).ToList();
        }
    }
}