using System.Collections.Generic;
using System.Linq;

namespace SeparateModels.Domain
{
    public class PolicyVersionComparer
    {
        private readonly PolicyVersion newVersion;
        private readonly PolicyVersion oldVersion;
        private readonly List<string> changes = new List<string>();

        public PolicyVersionComparer(PolicyVersion newVersion, PolicyVersion oldVersion)
        {
            this.newVersion = newVersion;
            this.oldVersion = oldVersion;
        }

        public static PolicyVersionComparer Of(PolicyVersion newVersion, PolicyVersion oldVersion)
        {
            return new PolicyVersionComparer(newVersion, oldVersion);
        }

        public List<string> Compare()
        {
            changes.Clear();
            
            //policy status
            if (newVersion.PolicyStatus != oldVersion.PolicyStatus)
            {
                changes.Add($"Policy status changes to {newVersion.PolicyStatus} from {oldVersion.PolicyStatus}");
            }
            
            //cover period
            if (newVersion.CoverPeriod.ValidFrom != oldVersion.CoverPeriod.ValidFrom)
            {
                changes.Add($"Policy start date changes to {newVersion.CoverPeriod.ValidFrom.ToShortDateString()} from {oldVersion.CoverPeriod.ValidFrom.ToShortDateString()}");
            }
            
            if (newVersion.CoverPeriod.ValidTo != oldVersion.CoverPeriod.ValidTo)
            {
                changes.Add($"Policy end date changes to {newVersion.CoverPeriod.ValidTo.ToShortDateString()} from {oldVersion.CoverPeriod.ValidTo.ToShortDateString()}");
            }
            
            //total premium
            if (newVersion.TotalPremium != oldVersion.TotalPremium)
            {
                changes.Add($"Policy total premium changes to {newVersion.TotalPremium} from {oldVersion.TotalPremium}");
            }
            
            //covers
            CompareCovers();

            return changes;
        }

        private void CompareCovers()
        {
            var coverChanges = new List<string>();
            
            //look for covers only in new
            var newCovers =
                newVersion
                    .Covers
                    .Where(c => oldVersion.Covers.All(oldC => oldC.Cover.Code != c.Cover.Code));

            foreach (var newCover in newCovers)
            {
                coverChanges.Add($"Cover added {newCover.Cover.Code} {newCover.CoverPeriod.ValidFrom.ToShortDateString()} {newCover.CoverPeriod.ValidTo.ToShortDateString()} {newCover.Amount}");
            }
            
            //look for covers only in old
            var oldCovers =
                oldVersion
                    .Covers
                    .Where(c => newVersion.Covers.All(newC => newC.Cover.Code != c.Cover.Code));

            foreach (var newCover in newCovers)
            {
                coverChanges.Add($"Cover removed {newCover.Cover.Code} {newCover.CoverPeriod.ValidFrom.ToShortDateString()} {newCover.CoverPeriod.ValidTo.ToShortDateString()} {newCover.Amount}");
            }


            //add to result
            if (coverChanges.Count > 0)
            {
                changes.AddRange(coverChanges);
            }
        }
    }
}