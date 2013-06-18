using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Policy;
using Castle.ActiveRecord;

namespace lejr_dk.Models
{
    public enum CampType { None, Putte, Boerne, Junior, Teen }

    [ActiveRecord]
    public class Camp : ActiveRecordBase<Camp>
    {
        public Camp()
        {
        }
        public Camp(CampType campType, DateTime beginTime, DateTime endTime,
            DateTime displayBegin, DateTime displayEnd,
            string description, string linkToProgram)
        {
            CampType = campType;
            BeginTime = beginTime;
            EndTime = endTime;
            DisplayBegin = displayBegin;
            DisplayEnd = displayEnd;
            Description = description;
            LinkToProgram = linkToProgram;
        }

        private int _id;

        [PrimaryKey]
        private int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Property]
        public CampType CampType { get; private set; }
        [Property]
        public DateTime BeginTime { get; private set; }
        [Property]
        public DateTime EndTime { get; private set; }
        [Property]
        public DateTime DisplayBegin { get; private set; }
        [Property]
        public DateTime DisplayEnd { get; private set; }
        [Property]
        public string LinkToProgram { get; private set; }
        [Property]
        public string Description { get; private set; }
    }
}