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
        public static List<T> ApplySequenceSorting<T>(this List<T> query) 
                    where T : ProjectGeneratorSequence                                                                       
        {
            return query.OrderBy(g => g.SeqId).ToList();
        }
        public static List<T> ApplyStateSequenceSorting<T>(this List<T> query) 
                    where T : StateInitialiserState
                                                                         
        {
            return query.OrderBy(g => g.OrderId).ToList();
        }
    }
    
}