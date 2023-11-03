﻿using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace Vermaat.Crm.Specflow
{
    public class EntityReferenceComparer : IEqualityComparer<EntityReference>
    {
        public bool Equals(EntityReference first, EntityReference second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }
            if (ReferenceEquals(first, null) ||
                ReferenceEquals(second, null))
            {
                return false;
            }
            return first.Id == second.Id;
        }

        public int GetHashCode(EntityReference obj)
        {
            if (obj == null)
                return 0;
            return obj.GetHashCode();
        }
    }
}
