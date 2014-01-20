using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenRastaSwagger
{
    public class UriParameterParser
    {
        private readonly Regex _paramRegex=new Regex(@"\{(\w+)\}");

        public UriParameterParser(string uri)
        {
            var index = uri.IndexOf('?');

            if (index > -1)
            {
                _pathParams=ParseParams(uri.Substring(0, index));
                _queryParams=ParseParams(uri.Substring(index));
            }
            else
            {
                _pathParams=ParseParams(uri);
            }
                
        }

        private List<string> ParseParams(string s)
        {
            var matches = _paramRegex.Matches(s);
            return (from Match match in matches select match.Groups[1].Value.ToLower()).ToList();
        }

        private readonly List<string> _pathParams;
        private readonly List<string> _queryParams;

        public bool HasPathParam(string name)
        {
            return (_pathParams!=null) && _pathParams.Contains(name.ToLower());
        }

        public bool HasQueryParam(string name)
        {
            return (_queryParams!=null) && _queryParams.Contains(name.ToLower());
        }

    }
}