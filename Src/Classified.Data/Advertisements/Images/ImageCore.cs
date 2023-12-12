using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classified.Data.Base;
using Classified.Domain.Entities;
using Classified.Domain.ViewModels.Image;

namespace Classified.Data.Advertisements.Images
{
    /// <summary>
    /// Core for Management of Image information in Database
    /// </summary>
    public class ImageCore:RepositoryBase<ImageViewModel,ClassifiedImages>
    {
    }
}
