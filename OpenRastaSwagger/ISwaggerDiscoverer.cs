using System;
using OpenRastaSwagger.Grouping;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger
{
    public interface ISwaggerDiscoverer
    {
        ResourceList GetResourceList();
        ResourceList GetResourceList(Func<OperationGroup, string> groupingOperation);
        ResourceDetails GetResouceDetails(string groupPath);
    }
}