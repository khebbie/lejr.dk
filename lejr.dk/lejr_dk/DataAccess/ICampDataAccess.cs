using System.Collections.Generic;
using lejr_dk.Models;
using NHibernate.Criterion;

namespace lejr_dk.DataAccess
{
    public interface ICampDataAccess
    {
        IList<Camp> GetCampsByQuery(Order[] orders, params ICriterion[] criteria);
    }
}