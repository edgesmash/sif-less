﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIFLess.Model.Profiles;

namespace SIFLess.Model.Managers
{
    public interface IProfileManager
    {
        SifLessProfiles Fetch();

        void Update(SifLessProfiles profileData);
    }
}
