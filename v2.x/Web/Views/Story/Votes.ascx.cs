﻿namespace Kigg.Web
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DomainObjects;

    public partial class Votes : ViewUserControl<ICollection<IVote>>
    {
    }
}