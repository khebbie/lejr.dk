using System.Collections.Generic;
using lejr_dk.Models;
using NHibernate.Criterion;

namespace lejr_dk.DataAccess
{
    public class CampDataAccess : ICampDataAccess
    {
        public IList<Camp> GetCampsByQuery(Order[] orders, params ICriterion[] criteria)
        {
            return Camp.FindAll(orders, criteria);
        }
    }
}