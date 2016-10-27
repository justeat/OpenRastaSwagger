using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenRastaSwagger
{
    public class UriParameterParser
    {
        public string Path { get; private set; }
        public string Query { get; private set; }

        public IEnumerable<string> Parameters
        {
            get { return _queryParams.Concat(_pathParams); }
        }

        private readonly List<string> _pathParams;
        private readonly List<string> _queryParams;
        private readonly Regex _paramRegex=new Regex(@"\{(\w+)\}");

        public UriParameterParser(string uri)
        {
            var index = uri.IndexOf('?');
            if (index > -1)
            {
                Path = uri.Substring(0, index).TrimEnd('/');
                Query = uri.Substring(index);
            }
            else
            {
                Path = uri;
                Query = "";
            }

            _pathParams = ParseParams(Path);
            _queryParams = ParseParams(Query);
        }

        private List<string> ParseParams(string s)
        {
            var matches = _paramRegex.Matches(s);
            return (from Match match in matches select match.Groups[1].Value.ToLower()).ToList();
        }

        public bool HasPathParam(string name)
        {
            return (_pathParams!=null) && _pathParams.Contains(name.ToLower());
        }

        public bool HasQueryParam(string name)
        {
            return (_queryParams!=null) && _queryParams.Contains(name.ToLower());
        }

        public bool HasParam(string name)
        {
            return HasQueryParam(name) || HasPathParam(name);
        }
    }
}