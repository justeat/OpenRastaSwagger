namespace OpenRastaSwagger.Model.Contracts
{
    public class Contract
    {
        public Contract()
        {
            operations = new JsonDictionary<string, Operation>();
            commonRequestHeaders = new JsonDictionary<string, HttpHeader>();
            commonResponseHeaders = new JsonDictionary<string, HttpHeader>();
            dependencies=new Dependencies();
            performanceMonitoring=new PerformanceMonitoring();
        }
        public string api { get; set; }
        public JsonDictionary<string, HttpHeader> commonRequestHeaders { get; set; }
        public JsonDictionary<string, HttpHeader> commonResponseHeaders { get; set; }
        public Dependencies dependencies { get; set; }
        public PerformanceMonitoring performanceMonitoring { get; set; }
        public string description { get; set; }
        public JsonDictionary<string, Operation> operations { get; set; }
        public string version { get; set; }

    }

    public class PerformanceMonitoring
    {
    }

    public class HttpHeader
    {
        public string description { get; set; }
        public string pattern { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
    }

    public class Dependencies
    {
        public Dependencies()
        {
            dotNetFramework=new Dotnetframework();
            iis=new Iis();
            memory=new Memory();
            operatingSystem=new Operatingsystem();
            processor=new Processor();
        }
        public Dotnetframework dotNetFramework { get; set; }
        public Iis iis { get; set; }
        public Memory memory { get; set; }
        public Operatingsystem operatingSystem { get; set; }
        public Processor processor { get; set; }
    }

    public class Dotnetframework
    {
        public string minVersion { get; set; }
    }

    public class Iis
    {
        public string appPool { get; set; }
        public string hostHeader { get; set; }
    }

    public class Memory
    {
        public int minGBAvailable { get; set; }
    }

    public class Operatingsystem
    {
        public string architecture { get; set; }
        public string minVersion { get; set; }
        public string type { get; set; }
    }

    public class Processor
    {
        public int minComputeUnitsAvailable { get; set; }
        public int minLogicalCoresAvailable { get; set; }
    }

    public class Operation
    {
        public Operation()
        {
            parameters = new JsonDictionary<string, Parameter>();
            maxResponseTime = new MaxResponseTime();
        }
        public string description { get; set; }
        public string method { get; set; }
        public JsonDictionary<string, Parameter> parameters { get; set; }
        public Returns returns { get; set; }
        public string status { get; set; }
        public string urlFormat { get; set; }
        public MaxResponseTime maxResponseTime { get; set; }
    }


    public class MaxResponseTime
    {
        public MaxResponseTime()
        {
            percentiles=new JsonDictionary<string, Time>();
            limits=new Limits();
        }


        public Limits limits { get; set; }
        public JsonDictionary<string, Time> percentiles { get; set; }
    }

    public class Time
    {
        public int milliseconds { get; set; }
    }

    public class Limits
    {
        public int maxRequestsPerSecond { get; set; }
    }

    public class Parameter
    {
        public string description { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public Schema schema { get; set; }
    }

    public class Returns
    {
        public string description { get; set; }
        public Schema schema { get; set; }
    }

    public class Schema
    {
        public Schema()
        {
            properties = new JsonDictionary<string, Parameter>();
        }
        public JsonDictionary<string, Parameter> properties { get; set; }
        public string type { get; set; }
    }

}
