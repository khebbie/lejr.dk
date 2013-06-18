using System.Collections.Generic;

namespace lejr_dk.Models
{
    public interface ICampService
    {
        IList<Camp> GetActiveCamps(CampType campType);
    }
}