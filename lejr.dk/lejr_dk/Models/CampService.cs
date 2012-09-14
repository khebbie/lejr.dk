using System;
using System.Collections.Generic;
using lejr_dk.DataAccess;
using NHibernate.Criterion;

namespace lejr_dk.Models
{
    public class CampService : ICampService
    {
        private readonly ICampDataAccess _campsDataAccess;

        public CampService(ICampDataAccess campsDataAccess)
        {
            _campsDataAccess = campsDataAccess;
        }

        #region ICampService Members

        public IList<Camp> GetActiveCamps(CampType campType)
        {
            IList<Camp> camps = _campsDataAccess.GetCampsByQuery(
                new[]
                    {
                        Order.Desc("BeginTime")
                    },
                new ICriterion[]
                    {
                        Restrictions.Eq("CampType", campType),
                        Restrictions.Ge("BeginTime", DateTime.Now)
                    });
            return camps;
        }

        #endregion
    }
}